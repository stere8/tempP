using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Pages.Account
{
    [Authorize] // Protect this page
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;

        public ProfileModel(UserManager<ApplicationUser> userManager, OnlineShopDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public ApplicationUser UserProfile { get; set; }

        public List<Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            UserProfile = user;

            Orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FirstName = UserProfile.FirstName;
            user.LastName = UserProfile.LastName;
            user.Address = UserProfile.Address;

            await _userManager.UpdateAsync(user);
            TempData["Message"] = "Profile updated successfully!";
            return RedirectToPage();
        }
    }
}

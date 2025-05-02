using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Services;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Models;
using System.Text.RegularExpressions;

namespace OnlineShop.Pages.Checkout
{
    public class CheckoutModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutModel(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        [BindProperty]
        public string PaymentMethod { get; set; }

        [BindProperty]
        public string CardNumber { get; set; }

        [BindProperty]
        public string ExpiryDate { get; set; }

        [BindProperty]
        public string CVV { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (PaymentMethod == "Card")
            {
                if (!Regex.IsMatch(ExpiryDate, @"^(0[1-9]|1[0-2])\\/\\d{2}$"))
                {
                    TempData["ErrorMessage"] = "Invalid expiry date format. Use MM/YY.";
                    return RedirectToPage("Failure");
                }

                string[] parts = ExpiryDate.Split('/');
                int expMonth = int.Parse(parts[0]);
                int expYear = 2000 + int.Parse(parts[1]);

                DateTime expiry = new DateTime(expYear, expMonth, DateTime.DaysInMonth(expYear, expMonth));
                if (expiry < DateTime.UtcNow)
                {
                    TempData["ErrorMessage"] = "Card has expired.";
                    return RedirectToPage("Failure");
                }

                //GET USER ID
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    TempData["ErrorMessage"] = "User not authenticated.";
                    return RedirectToPage("Failure");
                }

                try
                {
                    //CREATE ORDER
                    var order = await _orderService.CreateOrderFromCartAsync(userId);

                    TempData["Message"] = "Payment successful! Order placed.";
                    TempData["OrderId"] = order.OrderId;

                    return RedirectToPage("Success");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Order failed: {ex.Message}";
                    return RedirectToPage("Failure");
                }
            }
            else if (PaymentMethod == "PayPal")
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    TempData["ErrorMessage"] = "User not authenticated.";
                    return RedirectToPage("Failure");
                }

                try
                {
                    var order = await _orderService.CreateOrderFromCartAsync(userId);
                    TempData["Message"] = "PayPal payment simulated. Order placed.";
                    TempData["OrderId"] = order.OrderId;

                    return RedirectToPage("Success");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Order failed: {ex.Message}";
                    return RedirectToPage("Failure");
                }
            }


            TempData["ErrorMessage"] = "Invalid payment method.";
            return RedirectToPage("Failure");
        }
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sms.backend.Data;
using sms.backend.Services;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------
// Add services to the container.
// ---------------------------------------------------------------------

// Controllers with JSON options (preserve references and indent output)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});
builder.Services.AddAuthorization();

// Register DbContext and Identity
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SchoolContext>()
    .AddDefaultTokenProviders();

// Register custom services
builder.Services.AddScoped<ClassesService>();

// Configure CORS with specific origin (with credentials support)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});

// Configure logging to use console only
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// ---------------------------------------------------------------------
// Build the application.
// ---------------------------------------------------------------------

var app = builder.Build();


// ---------------------------------------------------------------------
// Configure the HTTP request pipeline.
// ---------------------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    // In development, show detailed error pages and enable Swagger.
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    // In production, use a custom exception handler that also sets the CORS header,
    // so error responses are not blocked by the browser.
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            // Set status code to 500 and add the CORS header for your frontend origin.
            context.Response.StatusCode = 500;
            context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");

            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature != null)
            {
                var exception = exceptionFeature.Error;
                // Write a generic error message (you can log the detailed exception instead)
                await context.Response.WriteAsync("An unexpected error occurred: " + exception.Message);
            }
        });
    });
    app.UseHsts();
}

// Uncomment the following if you want to enable HTTPS redirection in the future.
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

// IMPORTANT: Place UseCors after UseRouting so that all endpoints (including error responses)
// receive the correct CORS headers.
app.UseCors("AllowSpecificOrigin");

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to endpoints.
app.MapControllers();

// Run the application.
app.Run();

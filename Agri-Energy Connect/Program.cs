using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10378422_PROG7311_POE.Data;
using ST10378422_PROG7311_POE.Models;
using ST10378422_PROG7311_POE.Services;

namespace ST10378422_PROG7311_POE
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Dependency Injection ApplicationDbContext with SQL Server for the Agri-Energy Connect Platform

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity and link it to ApplicationUser and ApplicationDbContext

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure cookie authentication
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });


            builder.Services.AddAntiforgery(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });



            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(); 
            builder.Services.AddScoped<IRoleInitializer, RoleInitializer>();

            var app = builder.Build();

            // Initialize roles
            using (var scope = app.Services.CreateScope())
            {
                var roleInitializer = scope.ServiceProvider.GetRequiredService<IRoleInitializer>();
                await roleInitializer.InitializeRolesAsync();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); 
            app.UseRouting();
            app.UseCookiePolicy();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages(); 

            app.Run();
        }
    }
}

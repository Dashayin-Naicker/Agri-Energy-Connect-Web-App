using Microsoft.AspNetCore.Identity;
using ST10378422_PROG7311_POE.Data;
using ST10378422_PROG7311_POE.Models;

namespace ST10378422_PROG7311_POE.Services
{
    // RoleInitializer is responsible for creating predefined roles during app startup
    public class RoleInitializer : IRoleInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructor with dependency injection for RoleManager
        public RoleInitializer(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // This method checks if roles exist, and if not, creates them
        public async Task InitializeRolesAsync()
        {
            // Define required application roles (Employee and Farmer)
            string[] roleNames = { "Employee", "Farmer" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName)); // Create the role if it does not exist
                }
            }
        }
    }

    public interface IRoleInitializer
    {
        Task InitializeRolesAsync();
    }

}

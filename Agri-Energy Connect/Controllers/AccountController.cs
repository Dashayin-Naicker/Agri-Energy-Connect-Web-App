using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ST10378422_PROG7311_POE.Models;
using ST10378422_PROG7311_POE.Data;
using System.Threading.Tasks;
using System.Linq;

// Controller responsible for handling user registration, login, and logout
public class AccountController : Controller
{
    // Dependency injection of UserManager, SignInManager, and ApplicationDbContext
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    // Constructor to initialize dependencies
    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context)
    {
        _userManager = userManager; 
        _signInManager = signInManager;
        _context = context;
    }

    // GET: /Account/Register
    // Displays the registration form
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    // Handles form submission for new user registration
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        // Checks if the form input is valid
        if (ModelState.IsValid)
        {
            // Create a new ApplicationUser using the provided email
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            // Attempt to create the user with the given password
            var result = await _userManager.CreateAsync(user, model.Password);

            // Validate custom fields based on selected role
            if (model.Role == "Farmer" && string.IsNullOrWhiteSpace(model.FarmLocation))
            {
                ModelState.AddModelError("FarmLocation", "Farm Location is required for Farmers.");
            }
            else if (model.Role == "Employee" && string.IsNullOrWhiteSpace(model.Department))
            {
                ModelState.AddModelError("Department", "Department is required for Employees.");
            }

            // Proceed if user creation succeeded and custom validation passed
            if (result.Succeeded && ModelState.IsValid)
            {
                // Role-specific setup
                if (model.Role == "Farmer")
                {
                    // Assign role to user
                    await _userManager.AddToRoleAsync(user, "Farmer");

                    // Create farmer profile and link it to the user
                    var farmer = new Farmer
                    {
                        FullName = model.FullName,
                        FarmLocation = model.FarmLocation,
                        UserId = user.Id
                    };

                    _context.Farmers.Add(farmer);
                }
                else if (model.Role == "Employee")
                {
                    await _userManager.AddToRoleAsync(user, "Employee");

                    var employee = new Employee
                    {
                        FullName = model.FullName,
                        Department = model.Department,
                        UserId = user.Id
                    };

                    _context.Employees.Add(employee);
                }

                // Save user profile data to database
                await _context.SaveChangesAsync();

                // Auto-login user if no one is currently signed in
                if (!_signInManager.IsSignedIn(User))
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["SuccessMessage"] = "Registration successful! You are now logged in.";
                    return RedirectToAction("Index", "Home");
                }

                // If user was created by another logged-in user (e.g., Employee), just redirect
                TempData["SuccessMessage"] = "User registered successfully.";
                return RedirectToAction("ViewFarmers", "Employee");
            }

            // If errors occurred, display them
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // Re-display form with validation errors
        return View(model);
    }

    // GET: /Account/Login
    // Displays the login form
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    // Handles user login attempt
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Attempt to sign in the user (no persistent cookie)
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                // Retrieve user info and role
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);

                // Redirect based on role
                if (roles != null && roles.Count > 0)
                {
                    if (roles.Contains("Farmer"))
                    {
                        TempData["SuccessMessage"] = "Login successful! Welcome back.";
                        return RedirectToAction("Index", "Farmer");
                    }
                    else if (roles.Contains("Employee"))
                    {
                        TempData["SuccessMessage"] = "Login successful! Welcome back.";
                        return RedirectToAction("Index", "Employee");
                    }
                }

                // Default fallback redirect
                return RedirectToAction("Index", "Home");
            }

            // Login failed
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        // Show validation or login errors
        return View(model);
    }

    // POST: /Account/Logout
    // Handles user logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync(); // Log out user
        return RedirectToAction("Index", "Home"); // Redirect to home page
    }
}

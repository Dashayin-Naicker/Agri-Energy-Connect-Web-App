using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10378422_PROG7311_POE.Data;
using ST10378422_PROG7311_POE.Models;

namespace ST10378422_PROG7311_POE.Controllers
{
    // Restrict access to users with the "Employee" role only
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor injection for the application's database context
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays the "AddFarmer" form view
        // GET: Employee/AddFarmer
        public IActionResult AddFarmer()
        {
            return View();
        }

        // Handles form submission for adding a new Farmer
        // POST: Employee/AddFarmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            // If form data is valid
            if (ModelState.IsValid)
            {
                _context.Add(farmer);                    // Add farmer to the database context
                await _context.SaveChangesAsync();       // Save changes to the actual database
                return RedirectToAction(nameof(ViewFarmers)); // Redirect to the list of farmers
            }

            // If validation fails, re-display the form with validation messages
            return View(farmer);
        }

        // Displays the list of all Farmers
        // GET: Employee/ViewFarmers
        public async Task<IActionResult> ViewFarmers()
        {
            var farmers = await _context.Farmers.ToListAsync(); // Fetch all farmers from the database
            return View(farmers); // Pass the list to the view
        }

        // Filters products by farmer, category, and optional date range
        // GET: Employee/FilterProducts
        public async Task<IActionResult> FilterProducts(string farmerId, string category, DateTime? startDate, DateTime? endDate)
        {
            // Retrieve and send farmer list to the view for dropdown filter
            var farmers = await _context.Farmers.ToListAsync();
            ViewBag.Farmers = farmers;
            ViewBag.SelectedFarmerId = farmerId;

            // Start building the query for products, including their related Farmer
            var productsQuery = _context.Products.Include(p => p.Farmer).AsQueryable();

            // Apply farmer ID filter if provided and valid
            if (!string.IsNullOrEmpty(farmerId))
            {
                if (int.TryParse(farmerId, out int id))
                {
                    productsQuery = productsQuery.Where(p => p.FarmerId == id);
                }
            }

            // Apply category filter if provided
            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.Category.Contains(category));
            }

            // Filter by production date range if start and/or end dates are provided
            if (startDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate <= endDate.Value);
            }

            // Execute the query and return the filtered list
            var products = await productsQuery.ToListAsync();
            return View(products);
        }

        // GET: Employee/ViewFarmerProducts
        public async Task<IActionResult> ViewFarmerProducts()
        {
            var products = await _context.Products.Include(p => p.Farmer).ToListAsync();
            return View(products);
        }

        // POST: Employee/DeleteProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewFarmerProducts));
        }

        // Landing page for the Employee dashboard
        // GET: Employee/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}

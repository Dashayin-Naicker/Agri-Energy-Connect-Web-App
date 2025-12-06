using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10378422_PROG7311_POE.Data;
using ST10378422_PROG7311_POE.Models;
using System.Security.Claims;

namespace ST10378422_PROG7311_POE.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Farmer/AddProduct
        public IActionResult AddProduct()
        {
            var product = new Product
            {
                ProductionDate = DateTime.Now // Set default production date as today's date
            };
            return View(product);  // Pass the model to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            // Check if the ProductionDate is being passed correctly
            Console.WriteLine($"ProductionDate: {product.ProductionDate}");

            // Get the user Id from claims (using NameIdentifier)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Consistent user identification
            Console.WriteLine($"UserId: {userId}"); // Log for debugging

            if (userId == null)
            {
                // Handle case where UserId is not found
                return Unauthorized("User is not authenticated.");
            }

            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(f => f.UserId == userId);

            if (farmer == null)
            {
                // Log if farmer is not found
                Console.WriteLine("Farmer not found for the current user.");
                return NotFound("Farmer not found for the current user.");
            }

            product.FarmerId = farmer.FarmerId;
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyProducts));
        }

        // GET: Farmer/MyProducts
        public async Task<IActionResult> MyProducts()
        {
            // Get the user Id from claims (using NameIdentifier)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var farmer = await _context.Farmers
                .Include(f => f.Products) // Ensure related products are included
                .FirstOrDefaultAsync(f => f.UserId == userId);

            if (farmer != null)
            {
                return View(farmer.Products); // Return the list of products for the farmer
            }

            return NotFound("Farmer not found for the current user.");
        }

        // GET: Farmer/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}

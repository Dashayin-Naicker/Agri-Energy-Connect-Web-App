using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10378422_PROG7311_POE.Models;

namespace ST10378422_PROG7311_POE.Data
{
    //My Database Context Class
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet for farmers - represents the 'Farmers' table
        public DbSet<Farmer> Farmers { get; set; }

        // DbSet for products - represents the 'Products' table
        public DbSet<Product> Products { get; set; }

        // DbSet for employees - represents the 'Employees' table
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure 1-to-1 relationship between ApplicationUser and Farmer
            builder.Entity<ApplicationUser>()
                .HasOne(a => a.FarmerProfile)
                .WithOne(f => f.User)
                .HasForeignKey<Farmer>(f => f.UserId);

            // Configure 1-to-1 relationship between ApplicationUser and Employee
            builder.Entity<ApplicationUser>()
                .HasOne(a => a.EmployeeProfile)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserId);
        }

    }
}

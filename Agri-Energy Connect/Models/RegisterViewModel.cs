using System.ComponentModel.DataAnnotations;

namespace ST10378422_PROG7311_POE.Models
{
    // ViewModel for user registration. Supports both Farmer and Employee roles.
    public class RegisterViewModel : IValidatableObject
    {
        // Full name of the user 
        [Required]
        public string FullName { get; set; }

        // Email address of the user (must be in valid email format)

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Password for the account

        [Required]
        public string Password { get; set; }

        // Must match the Password field 

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        // Role of the user: either "Farmer" or "Employee" 

        [Required]
        public string Role { get; set; }

        // Optional farm location – required only if Role is "Farmer"
        public string? FarmLocation { get; set; }

        // Optional department – required only if Role is "Employee"
        public string? Department { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Role == "Farmer" && string.IsNullOrWhiteSpace(FarmLocation))
            {
                yield return new ValidationResult("FarmLocation is required for Farmers.", new[] { nameof(FarmLocation) });
            }

            if (Role == "Employee" && string.IsNullOrWhiteSpace(Department))
            {
                yield return new ValidationResult("Department is required for Employees.", new[] { nameof(Department) });
            }
        }
    }

}

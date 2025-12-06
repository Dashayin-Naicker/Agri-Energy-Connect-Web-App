using Microsoft.AspNetCore.Identity;

namespace ST10378422_PROG7311_POE.Models
{

    public class ApplicationUser : IdentityUser
    {
        public virtual Farmer FarmerProfile { get; set; }
        public virtual Employee EmployeeProfile { get; set; }
    }
}

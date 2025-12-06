using System.ComponentModel.DataAnnotations;

namespace ST10378422_PROG7311_POE.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}

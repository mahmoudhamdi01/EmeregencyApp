using System.ComponentModel.DataAnnotations;

namespace Project.API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*])[A-Za-z\\d!@#$%^&*]{6,12}$", ErrorMessage = 
            "Password must Contain 1 Uppercase, 1 Lowercase, 1 Digit, I Special Character ")]
        public string Password { get; set; }
    }
}

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
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
    ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character.")]
        public string Password { get; set; }

    }
}

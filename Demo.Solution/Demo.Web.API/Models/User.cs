using System.ComponentModel.DataAnnotations;

namespace Demo.Web.API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Name cannot be empty or null")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name should have 3 to 40 chars")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name must contain only letters and spaces")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email cannot be empty or null")]
        [EmailAddress(ErrorMessage = "Verify your email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty or null")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password should have 8 to 40 chars")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirmed Password cannot be empty or null")]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string? ConfirmedPassword { get; set; }

        [Required(ErrorMessage = "Role cannot be empty or null")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Role must contain only letters and spaces")]
        public string? Role { get; set; }
    }
}

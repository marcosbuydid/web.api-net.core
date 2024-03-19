using System.ComponentModel.DataAnnotations;

namespace Demo.Web.API.Models
{
    public record UserDTO
    {
        [Required(ErrorMessage = "Email cannot be empty or null")]
        [EmailAddress(ErrorMessage = "Verify your email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty or null")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password should have 8 to 40 chars")]
        public string Password { get; set; }
    }
}

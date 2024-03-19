using System.ComponentModel.DataAnnotations;

namespace Demo.Web.API.Models
{
    public class Session
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Token cannot be empty or null")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Email cannot be empty or null")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Creation date cannot be empty or null")]
        public DateTime CreatedAt { get; set; }
    }
}

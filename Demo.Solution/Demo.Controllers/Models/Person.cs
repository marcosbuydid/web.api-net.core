using System.ComponentModel.DataAnnotations;

namespace Demo.Controllers.Models
{
    public class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "First Name cannot be empty or null")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "FirstName should have 3 to 40 chars")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "FirstName must contain only letters and spaces")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name cannot be empty or null")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "LastName should have 3 to 40 chars")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "FirstName must contain only letters and spaces")]
        public string? LastName { get; set; }

        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email cannot be empty or null")]
        [EmailAddress(ErrorMessage ="Verify your email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty or null")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password should have 8 to 40 chars")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirmed Password cannot be empty or null")]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string? ConfirmedPassword { get; set; }
    }

}

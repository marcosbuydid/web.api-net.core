using System.ComponentModel.DataAnnotations;

namespace Demo.Controllers.Models
{
    public class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "First Name cannot be empty or null")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last Name cannot be empty or null")]
        public string? LastName { get; set; }
        public int Age { get; set; }
    }

}

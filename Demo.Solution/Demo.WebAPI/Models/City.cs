using System.ComponentModel.DataAnnotations;

namespace Demo.WebAPI.Models
{
    public class City
    {
        [Key]
        public Guid CityID { get; set; }

        [Required(ErrorMessage ="City Name cannot be blank")]
        public string? CityName { get; set; }
    }
}

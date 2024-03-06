using System.ComponentModel.DataAnnotations;

namespace Demo.WebAPI.Models
{
    public class City
    {
        [Key]
        public Guid CityID { get; set; }

        public string? CityName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User
{
    public class AreaRequest
    {
        [Required]
        public string Id { get; set; }

        public string CountryId { get; set; }

        public string CountryName { get; set; }

        public string CityId { get; set; }

        public string CityName { get; set; }
    }
}

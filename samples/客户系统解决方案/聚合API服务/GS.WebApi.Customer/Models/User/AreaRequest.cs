using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.WebApi.Customer.Models.User
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

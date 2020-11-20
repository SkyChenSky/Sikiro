using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User
{
    public class UserLogoRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string ImgUrl { get; set; }
    }
}

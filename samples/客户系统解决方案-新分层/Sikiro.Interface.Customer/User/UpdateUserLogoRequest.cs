using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class UpdateUserLogoRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string ImgUrl { get; set; }
    }
}

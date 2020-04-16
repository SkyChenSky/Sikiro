using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User
{
    public class EmailRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }
    }
}

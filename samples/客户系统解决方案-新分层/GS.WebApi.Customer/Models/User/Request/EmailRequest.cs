using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User.Request
{
    public class EmailRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }
    }
}

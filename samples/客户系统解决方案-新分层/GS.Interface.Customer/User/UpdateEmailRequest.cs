using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class UpdateEmailRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }
    }
}

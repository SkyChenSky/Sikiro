using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class ResetUserPasswordRequest
    {
        [Required]
        public string Id { get; set; }

    }
}

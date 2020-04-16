using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class UpdateBusinessManagerRequest
    {
        [Required]
        public string UserId { get; set; }

        public string BusinessManager { get; set; }
    }
}

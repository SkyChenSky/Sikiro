using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class UpdateUserStatusRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public int Status { get; set; }
    }
}

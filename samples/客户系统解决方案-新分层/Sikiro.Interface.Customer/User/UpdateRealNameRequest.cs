using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class UpdateRealNameRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string RealName { get; set; }
    }
}

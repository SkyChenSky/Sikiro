using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User.Request
{
    public class UserNameRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}

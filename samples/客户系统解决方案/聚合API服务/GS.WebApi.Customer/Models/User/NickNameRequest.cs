using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User
{
    public class NickNameRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string NickName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class UpdateUserLableRequest
    {
        [Required]
        public string Id { get; set; }

        public string UserLable { get; set; }
    }
}

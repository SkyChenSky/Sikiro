using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class UpdateNickNameRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string NickName { get; set; }
    }
}

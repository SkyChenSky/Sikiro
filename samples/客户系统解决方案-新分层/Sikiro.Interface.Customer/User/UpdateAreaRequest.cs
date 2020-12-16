using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class UpdateAreaRequest
    {
        [Required]
        public string Id { get; set; }

        public UpdateArea AreaRequest {get; set; }
    }

}

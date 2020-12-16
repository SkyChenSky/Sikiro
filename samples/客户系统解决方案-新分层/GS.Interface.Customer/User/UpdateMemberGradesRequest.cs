using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class UpdateMemberGradesRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string MemberGradesId { get; set; }

        [Required]
        public string MemberGradesName { get; set; }
    }
}

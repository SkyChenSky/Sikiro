using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
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

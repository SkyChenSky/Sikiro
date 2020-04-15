using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class UpdateUserStatusRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public int Status { get; set; }
    }
}

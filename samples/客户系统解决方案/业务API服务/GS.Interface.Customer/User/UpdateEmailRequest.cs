using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class UpdateEmailRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }
    }
}

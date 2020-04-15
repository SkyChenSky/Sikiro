using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class ResetUserPasswordRequest
    {
        [Required]
        public string Id { get; set; }

    }
}

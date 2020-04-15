using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class UpdateBusinessManagerRequest
    {
        [Required]
        public string UserId { get; set; }

        public string BusinessManager { get; set; }
    }
}

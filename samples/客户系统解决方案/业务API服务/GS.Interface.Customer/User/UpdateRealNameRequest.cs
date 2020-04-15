using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class UpdateRealNameRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string RealName { get; set; }
    }
}

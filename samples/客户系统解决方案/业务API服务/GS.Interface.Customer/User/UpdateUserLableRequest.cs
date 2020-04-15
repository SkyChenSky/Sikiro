using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class UpdateUserLableRequest
    {
        [Required]
        public string Id { get; set; }

        public string UserLable { get; set; }
    }
}

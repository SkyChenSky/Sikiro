using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.WebApi.Customer.Models.User
{
    public class UserLogoRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string UserLogo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class UpdateUserLogoRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string ImgUrl { get; set; }
    }
}

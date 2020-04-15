using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class UpdateAreaRequest
    {
        [Required]
        public string Id { get; set; }

        public UpdateArea AreaRequest {get; set; }
    }

}

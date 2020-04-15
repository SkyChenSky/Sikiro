using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class SearchUserRequest
    {
        public string UserNo { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

        public DateTime? DateTimeFrom { get; set; }

        public DateTime? DateTimeTo { get; set; }

        public int Status { get; set; }

        public string CompanyId { get; set; }
    }
}

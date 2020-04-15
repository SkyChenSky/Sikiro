using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    public class FinancialBalancePayRequest
    {
        [Required(ErrorMessage = "订单ID为必传")]
        public string WharehouseOrderId { get; set; }


        public decimal Money { get; set; }

        public string UserId { get; set; }

        public string CompanyId { get; set; }
    }
}

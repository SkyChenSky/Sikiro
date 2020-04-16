using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User
{
    public class BalancePayRequest
    {
        [Required(ErrorMessage = "订单ID为必传")]
        public string WharehouseOrderId { get; set; }

        
        public decimal Money { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserWxPayDataRequest
    {
        /// <summary>
        /// 金额（分）
        /// </summary>
        [Required(ErrorMessage = "金额必传")]
        public int Money { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public string WharehouseOrderId { get; set; }
    }
}

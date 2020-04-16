using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserQueryWxPayDataRequest
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [Required(ErrorMessage = "流水号必传")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public string WharehouseOrderId { get; set; }
    }
}

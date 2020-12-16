using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User.Request
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UserCheckingPayPasswordRequest
    {
        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "请输入支付密码")]
        public string PayPassword { get; set; }
    }
}

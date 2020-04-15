using System.ComponentModel.DataAnnotations;

namespace GS.WebApi.Customer.Models.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserSetPayPwdRequest
    {
        /// <summary>
        /// 支付密码
        /// </summary>
        [Required(ErrorMessage = "请输入支付密码")]
        public string PayPassword { get; set; }
    }
}

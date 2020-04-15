using System.ComponentModel.DataAnnotations;
using GS.WebApi.Customer.Attribute;

namespace GS.WebApi.Customer.Models.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserRetrieveSetPayPwdRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "请输入手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        [Required(ErrorMessage = "请输入支付密码")]
        public string PayPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "请输入验证码")]
        public string Code { get; set; }
    }
}

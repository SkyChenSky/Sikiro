using System.ComponentModel.DataAnnotations;

namespace GS.WebApi.Customer.Models.User
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

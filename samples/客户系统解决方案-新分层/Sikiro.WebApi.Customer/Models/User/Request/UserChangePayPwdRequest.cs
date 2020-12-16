using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User.Request
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UserChangePayPwdRequest
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required(ErrorMessage = "请输入登录密码")]
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "请输入新登录密码")]
        public string NewPassword { get; set; }
    }
}

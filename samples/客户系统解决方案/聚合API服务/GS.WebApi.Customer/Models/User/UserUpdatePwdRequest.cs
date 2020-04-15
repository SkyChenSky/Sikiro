using System.ComponentModel.DataAnnotations;

namespace GS.WebApi.Customer.Models.User
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UserUpdatePwdRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required(ErrorMessage = "请输入用户ID")]
        public string UserId { get; set; }

        /// <summary>
        /// 登录密码（旧）
        /// </summary>
        [Required(ErrorMessage = "请输入登录密码")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新登录密码
        /// </summary>
        [Required(ErrorMessage = "请输入新登录密码")]
        public string NewPassword { get; set; }
    }
}

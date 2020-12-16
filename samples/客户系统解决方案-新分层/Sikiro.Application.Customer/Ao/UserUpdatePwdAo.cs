using System.ComponentModel.DataAnnotations;

namespace Sikiro.Application.Customer.Ao
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UserUpdatePwdAo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 登录密码（旧）
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 新登录密码
        /// </summary>
        public string NewPassword { get; set; }
    }
}

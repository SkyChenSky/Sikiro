using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GS.Interface.Customer.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdatePwdUserRequest
    {
        [Required(ErrorMessage = "请输入用户ID")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "请输入登录密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "请输入新登录密码")]
        public string NewPassword { get; set; }
    }
}

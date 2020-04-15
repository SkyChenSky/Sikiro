using System.ComponentModel.DataAnnotations;

namespace GS.Interface.Customer.User
{
    /// <summary>
    /// 
    /// </summary>
    public class SetPayPwdUserRequest
    {
        [Required(ErrorMessage = "请输入用户ID")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "请输入新支付密码")]
        public string PayPassword { get; set; }
    }
}

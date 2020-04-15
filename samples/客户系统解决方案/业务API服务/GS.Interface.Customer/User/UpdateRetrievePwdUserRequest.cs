using System.ComponentModel.DataAnnotations;

namespace GS.Interface.Customer.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateRetrievePwdUserRequest
    {
        [Required(ErrorMessage = "请输入手机号")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "请输入新登录密码")]
        public string NewPassword { get; set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        [Required(ErrorMessage = "请输入新登录密码")]
        public string CompanyId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "请输入用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        public string CompanyId { get; set; }

        [Required(ErrorMessage = "请输入手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        [Required(ErrorMessage = "区号")]
        public string CountryCode { get; set; }

        public string UserNo { get; set; }
    }
}

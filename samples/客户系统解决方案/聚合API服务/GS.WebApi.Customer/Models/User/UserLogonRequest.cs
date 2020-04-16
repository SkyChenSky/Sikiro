using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public class UserLogonRequest
    {
        /// <summary>
        /// 用户账户
        /// </summary>
        [Required(ErrorMessage = "请输入登录账号")]
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "请输入登录密码")]
        public string Password { get; set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        [Required(ErrorMessage = "公司Id不能为空")]
        public string CompanyId { get; set; }
    }
}

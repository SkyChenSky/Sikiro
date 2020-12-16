using System.ComponentModel.DataAnnotations;

namespace Sikiro.Application.Customer.Ao
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public class UserLogonInputAo
    {
        /// <summary>
        /// 用户账户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        public string CompanyId { get; set; }
    }
}

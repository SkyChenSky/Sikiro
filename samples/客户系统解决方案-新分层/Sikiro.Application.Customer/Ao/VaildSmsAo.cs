using System.ComponentModel.DataAnnotations;

namespace Sikiro.Application.Customer.Ao
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public class VaildSmsAo
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }
}

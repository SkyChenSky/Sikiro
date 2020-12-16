namespace Sikiro.Application.Customer.Ao
{
    /// <summary>
    /// 注册请求
    /// </summary>
    public class UserRegisterInputAo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserNo { get; set; }
    }
}

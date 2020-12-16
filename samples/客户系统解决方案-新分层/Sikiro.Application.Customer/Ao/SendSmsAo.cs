namespace Sikiro.Application.Customer.Ao
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public class SendSmsAo
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip { get; set; }
    }
}

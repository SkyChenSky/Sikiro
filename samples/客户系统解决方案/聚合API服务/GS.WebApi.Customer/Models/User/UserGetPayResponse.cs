namespace Sikiro.WebApi.Customer.Models.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserGetPayResponse
    {
        /// <summary>
        /// 公众号ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 微信时间
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 随机码
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// 扩展包
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string PaySign { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string OrderNo { get; set; }
    }
}

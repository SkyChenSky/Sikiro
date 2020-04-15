namespace GS.WebApi.Customer.Models.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserGetResponse
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 微信名称
        /// </summary>
        public string WxName { get; set; }
        /// <summary>
        /// LOGO
        /// </summary>
        public string UserLogo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 未读数
        /// </summary>
        public int NoReadCount { get; set; }

        /// <summary>
        /// 是否有支付密码
        /// </summary>
        public bool IsPayPwd { get; set; }

        /// <summary>
        /// 是否修改过用户名
        /// </summary>
        public bool IsChangedUserName { get; set; }
    }
}

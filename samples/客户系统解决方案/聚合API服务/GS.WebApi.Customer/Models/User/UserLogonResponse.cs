namespace GS.WebApi.Customer.Models.User
{
    /// <summary>
    /// 登录响应
    /// </summary>
    public class UserLogonResponse
    {
        
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        

        /// <summary>
        /// 授权码
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户WXid
        /// </summary>
        public string OpenId { get; set; }


        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 登录令牌
        /// </summary>
        public string Token { get; set; }
    }
}

namespace Sikiro.WebApi.Customer.Models.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoResponse
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// LOGO
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 国家ID
        /// </summary>
        public string CountryId { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public string CityId { get; set; }
        /// <summary>
        /// 是否修改过用户名
        /// </summary>
        public bool IsChangedUserName { get; set; }
    }
}

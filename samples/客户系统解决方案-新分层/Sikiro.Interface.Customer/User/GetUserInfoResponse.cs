using System;

namespace Sikiro.Interface.Customer.User
{
    /// <summary>
    /// 收件人地址
    /// </summary>
    public class GetUserInfoResponse
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 国家手机编码
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 微信Id
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WxName { get; set; }

        /// <summary>
        /// 头像url
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 状态（0:stop,1启用）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否黑名单
        /// </summary>
        public bool IsBlack { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDatetime { get; set; }

        /// <summary>
        /// 聊天备注
        /// </summary>
        public string ChatRemark { get; set; }

        /// <summary>
        /// 是否已修改用户名
        /// </summary>
        public bool IsChangedUsername { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 国家id
        /// </summary>
        public string CountryId { get; set; }

        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 会员等级id
        /// </summary>
        public string MemberGradesId { get; set; }
    }
}

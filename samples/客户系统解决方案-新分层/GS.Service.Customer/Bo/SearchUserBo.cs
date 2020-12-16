using System;

namespace Sikiro.Service.Customer.Bo
{
    public class SearchUserBo
    {
        public string UserId { get; set; }

        public string UserNo { get; set; }

        public string NickName { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

        public string OpenId { get; set; }

        public string WxName { get; set; }

        public string ImgUrl { get; set; }

        public string CompanyId { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public DateTime? CreateDatetime { get; set; }

        public string ChatRemark { get; set; }

        public string RealName { get; set; }

        public string Email { get; set; }

        public string CountryId { get; set; }

        public string CountryName { get; set; }

        public string CityId { get; set; }

        public string CityName { get; set; }

        public string Userlable { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        public string BusinessManager { get; set; }

        /// <summary>
        /// 会员等级id
        /// </summary>
        public string MemberGradesId { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public string VipLv { get; set; }
    }
}

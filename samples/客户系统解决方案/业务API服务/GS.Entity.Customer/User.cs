
/*
 * 本文件由根据实体插件自动生成，请勿更改
 * =========================== */

using System;
using Chloe.Annotations;

namespace GS.Entity.Customer
{
    [Table("user")]
    public class User
    {
        
        /// <summary>
        /// 主键
        /// </summary>
        [Column("user_id")]
        public string  UserId{ get; set; }
        
        /// <summary>
        /// 用户编号
        /// </summary>
        [Column("user_no")]
        public string  UserNo{ get; set; }
        
        /// <summary>
        /// 昵称
        /// </summary>
        [Column("nick_name")]
        public string  NickName{ get; set; }
        
        /// <summary>
        /// 登录用户名
        /// </summary>
        [Column("user_name")]
        public string  UserName{ get; set; }
        
        /// <summary>
        /// 国家手机编码
        /// </summary>
        [Column("country_code")]
        public string  CountryCode{ get; set; }
        
        /// <summary>
        /// 手机号
        /// </summary>
        [Column("phone")]
        public string  Phone{ get; set; }
        
        /// <summary>
        /// 密码
        /// </summary>
        [Column("password")]
        public string  Password{ get; set; }
        
        /// <summary>
        /// 微信Id
        /// </summary>
        [Column("open_id")]
        public string  OpenId{ get; set; }
        
        /// <summary>
        /// 微信昵称
        /// </summary>
        [Column("wx_name")]
        public string  WxName{ get; set; }
        
        /// <summary>
        /// 头像url
        /// </summary>
        [Column("img_url")]
        public string  ImgUrl{ get; set; }
        
        /// <summary>
        /// 企业Id
        /// </summary>
        [Column("company_id")]
        public string  CompanyId{ get; set; }
        
        /// <summary>
        /// 状态（0:stop,1启用）
        /// </summary>
        [NonAutoIncrement]
        [Column("status")]
        public int  Status{ get; set; }
        
        /// <summary>
        /// 是否黑名单
        /// </summary>
        [Column("is_black")]
        public bool  IsBlack{ get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_datetime")]
        public DateTime  CreateDatetime{ get; set; }
        
        /// <summary>
        /// 聊天备注
        /// </summary>
        [Column("chat_remark")]
        public string  ChatRemark{ get; set; }
        
        /// <summary>
        /// 是否已修改用户名
        /// </summary>
        [Column("is_changed_username")]
        public bool  IsChangedUsername{ get; set; }
        
        /// <summary>
        /// 真实名称
        /// </summary>
        [Column("real_name")]
        public string  RealName{ get; set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        [Column("email")]
        public string  Email{ get; set; }
        
        /// <summary>
        /// 国家id
        /// </summary>
        [Column("country_id")]
        public string  CountryId{ get; set; }
        
        /// <summary>
        /// 国家名称
        /// </summary>
        [Column("country_name")]
        public string  CountryName{ get; set; }
        
        /// <summary>
        /// 城市Id
        /// </summary>
        [Column("city_id")]
        public string  CityId{ get; set; }
        
        /// <summary>
        /// 城市名称
        /// </summary>
        [Column("city_name")]
        public string  CityName{ get; set; }
        
        /// <summary>
        /// 支付密码
        /// </summary>
        [Column("pay_password")]
        public string  PayPassword{ get; set; }
        
        /// <summary>
        /// 余额
        /// </summary>
        [Column("balance")]
        public decimal  Balance{ get; set; }
        
        /// <summary>
        /// 注册渠道
        /// </summary>
        [Column("registration_channel")]
        public string  RegistrationChannel{ get; set; }
        
        /// <summary>
        /// 身份证号
        /// </summary>
        [Column("id_num")]
        public string  IdNum{ get; set; }
        
        /// <summary>
        /// 邮编
        /// </summary>
        [Column("zip_code")]
        public string  ZipCode{ get; set; }
        
        /// <summary>
        /// 积分
        /// </summary>
        [NonAutoIncrement]
        [Column("integral")]
        public int  Integral{ get; set; }
        
        /// <summary>
        /// 会员等级
        /// </summary>
        [Column("vip_lv")]
        public string  VipLv{ get; set; }
        
        /// <summary>
        /// 专属客服
        /// </summary>
        [Column("exclusive_customer_service")]
        public string  ExclusiveCustomerService{ get; set; }
        
        /// <summary>
        /// 推荐人
        /// </summary>
        [Column("reference")]
        public string  Reference{ get; set; }
        
        /// <summary>
        /// 业务员
        /// </summary>
        [Column("business_manager")]
        public string  BusinessManager{ get; set; }
        
        /// <summary>
        /// 身份标签
        /// </summary>
        [Column("userlable")]
        public string  Userlable{ get; set; }
        
        /// <summary>
        /// 会员等级id
        /// </summary>
        [Column("member_grades_id")]
        public string  MemberGradesId{ get; set; }
    }
}

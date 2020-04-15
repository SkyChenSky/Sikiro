using System;
using GS.Nosql.Mongo.Base;

namespace GS.Infrastructure.Msg.Entitys
{
    /// <summary>
    /// 发送短信
    /// </summary>
    [Mongo(DbConfig.Name)]
    public class SmsRecord : MongoEntity
    {
        /// <summary>
        /// 接收的手机号码
        /// </summary>
        public string PhoneNum { get; set; }

        /// <summary>
        /// 短信内容，例如验证码等 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string IpAddress { get; set; }
    }

    public class DbConfig
    {
        public const string Name = "geshiimdb";
    }
}

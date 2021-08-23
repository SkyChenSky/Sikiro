namespace Sikiro.ES.Api.Model.UserViewRecord
{
    /// <summary>
    /// 新增
    /// </summary>
    public class UserViewRecordPostRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 作品ID
        /// </summary>
        public long EntityId { get; set; }

        /// <summary>
        /// 作品类型
        /// </summary>
        public long EntityType { get; set; }

        /// <summary>
        /// 章节ID
        /// </summary>
        public long CharpterId { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip { get; set; }
    }
}

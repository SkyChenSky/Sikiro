using System;

namespace Sikiro.ES.Api.Model.UserViewDuration
{
    /// <summary>
    /// 列表
    /// </summary>
    public class UserViewDurationRecordGetResponse
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 作品ID
        /// </summary>
        public long? EntityId { get; set; }

        /// <summary>
        /// 章节ID
        /// </summary>
        public long? CharpterId { get; set; }

        /// <summary>
        /// 作品类型 ,1=漫画,2=小说,3=音频,4=对话小说
        /// </summary>
        public long? EntityType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }

    }
}

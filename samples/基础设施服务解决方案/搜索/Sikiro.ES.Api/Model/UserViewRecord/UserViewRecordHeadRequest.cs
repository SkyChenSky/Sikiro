using System;

namespace Sikiro.ES.Api.Model.UserViewRecord
{
    /// <summary>
    /// 总数
    /// </summary>
    public class UserViewRecordHeadRequest : BaseRequest
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
        /// 作品类型
        /// </summary>
        public long EntityType { get; set; }

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

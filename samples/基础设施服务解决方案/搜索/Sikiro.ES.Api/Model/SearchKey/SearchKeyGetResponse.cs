using System.Collections.Generic;

namespace Sikiro.ES.Api.Model.SearchKey
{
    /// <summary>
    /// 列表
    /// </summary>
    public class SearchKeyGetResponse
    {
        public int KeyId { get; set; }

        /// <summary>
        /// 作品ID
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// 作品类型
        /// </summary>
        public int EntityType { get; set; }

        /// <summary>
        /// 搜索键
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 是否描述
        /// </summary>
        public bool IsSubsidiary { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public List<int> SysTagId { get; set; }
    }
}

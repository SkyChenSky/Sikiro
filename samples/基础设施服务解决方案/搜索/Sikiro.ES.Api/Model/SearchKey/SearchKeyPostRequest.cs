using System;
using System.Collections.Generic;

namespace Sikiro.ES.Api.Model.SearchKey
{
    /// <summary>
    /// 新增
    /// </summary>
    public class SearchKeyPostRequest
    {
        public string IndexName { get; set; }

        public List<SearchKeyPostItem> Items { get; set; }
    }

    public class SearchKeyPostItem
    {
        /// <summary>
        /// 主键
        /// </summary>
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
        public int Weight { get; set; }

        /// <summary>
        /// 是否描述
        /// </summary>
        public bool IsSubsidiary { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public List<int> SysTagId { get; set; }

        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime? ActiveDate { get; set; }
    }
}

using System.Collections.Generic;
using Sikiro.ES.Api.Model.SearchKey.Enum;

namespace Sikiro.ES.Api.Model.SearchKey
{
    /// <summary>
    /// 列表
    /// </summary>
    public class SearchKeyCountRequest
    {
        /// <summary>
        /// 搜索键
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// 是否描述
        /// </summary>
        public bool? IsSubsidiary { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public List<int> SysTagIds { get; set; }

        /// <summary>
        /// 作品类型
        /// </summary>
        public ESearchKey.EntityType? EntityType { get; set; }
    }
}

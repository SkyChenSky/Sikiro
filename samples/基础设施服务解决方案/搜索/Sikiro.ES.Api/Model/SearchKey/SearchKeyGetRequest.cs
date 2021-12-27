using System.Collections.Generic;
using Sikiro.ES.Api.Model.SearchKey.Enum;

namespace Sikiro.ES.Api.Model.SearchKey
{
    /// <summary>
    /// 列表
    /// </summary>
    public class SearchKeyGetRequest
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
        /// 排序
        /// </summary>
        public ESearchKey.Sort Sort { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

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

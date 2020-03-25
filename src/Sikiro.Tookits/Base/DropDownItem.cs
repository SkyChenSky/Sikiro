namespace Sikiro.Tookits.Base
{
    /// <summary>
    /// 下拉框元素
    /// </summary>
    public class DropDownItem
    {
        /// <summary>  
        /// 枚举的描述  
        /// </summary>  
        public string Text { set; get; }

        /// <summary>  
        /// 枚举对象的值  
        /// </summary>  
        public object Value { set; get; }

        /// <summary>
        /// 父ID
        /// </summary>
        public object ParentId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string Prompt { get; set; }
    }
}

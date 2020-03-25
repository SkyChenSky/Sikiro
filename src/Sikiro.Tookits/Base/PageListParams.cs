namespace Sikiro.Tookits.Base
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageListParams
    {
        private int? _limit;
        /// <summary>
        /// 页长
        /// </summary>
        public int Limit
        {
            set => _limit = value;
            get => _limit ?? 10;
        }

        private int? _page;
        /// <summary>
        /// 页码
        /// </summary>
        public int Page
        {
            set => _page = value;
            get => _page ?? 1;
        }
    }

    public class PageListParams<TParam> : PageListParams where TParam : new()
    {
        public PageListParams()
        {
            Params = new TParam();
        }

        /// <summary>
        /// 搜索参数
        /// </summary>
        public TParam Params { get; set; }
    }
}

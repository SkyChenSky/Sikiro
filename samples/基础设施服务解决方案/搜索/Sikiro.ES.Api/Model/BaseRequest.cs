using System.ComponentModel.DataAnnotations;

namespace Sikiro.ES.Api.Model
{
    public abstract class BaseRequest
    {
        private int? _size;
        /// <summary>
        /// 长度
        /// </summary>
        [Range(0, 1000)]
        public int Size
        {
            get => _size ?? 10;
            set => _size = value;
        }

        /// <summary>
        /// 上次查询的时间戳
        /// </summary>
        public long? Timestamp { get; set; }
    }
}

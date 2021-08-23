namespace Sikiro.ES.Api.Model
{
    public abstract class BaseRequest
    {
        private int? _size;
        public int Size
        {
            get => _size ?? 10;
            set => _size = value;
        }

        public long? Timestamp { get; set; }
    }
}

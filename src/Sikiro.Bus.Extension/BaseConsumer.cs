namespace Sikiro.Bus.Extension
{
    public abstract class BaseConsumer<T> where T : EasyNetQEntity
    {
        public abstract void Excute(T msg);
    }
}

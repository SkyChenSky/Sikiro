namespace Sikiro.Bus.Extension
{
    public abstract class BaseConsumer
    {
        public abstract void Excute<T>(T msg) where T : EasyNetQEntity;
    }
}

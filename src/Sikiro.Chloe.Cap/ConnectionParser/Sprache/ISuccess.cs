namespace ConnectionParser.Sprache
{
    internal interface ISuccess<out T> : IResult<T>
    {
        T Result { get; }
        Input Remainder { get; }
    }
}

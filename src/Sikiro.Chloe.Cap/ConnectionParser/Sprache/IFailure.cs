using System.Collections.Generic;

namespace Sikiro.Chloe.Cap.ConnectionParser.Sprache
{
    internal interface IFailure<out T> : IResult<T>
    {
        string Message { get; }
        IEnumerable<string> Expectations { get; }
        Input FailedInput { get; }
    }
}

using System;

namespace Sikiro.Chloe.Cap.ConnectionParser.Sprache
{
    internal class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        {
        }
    }
}

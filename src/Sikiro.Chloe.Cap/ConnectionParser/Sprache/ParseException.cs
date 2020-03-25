using System;

namespace ConnectionParser.Sprache
{
    internal class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        {
        }
    }
}

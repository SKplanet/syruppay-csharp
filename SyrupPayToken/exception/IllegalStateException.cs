using System;

namespace SyrupPayToken.exception
{
    public class IllegalStateException : Exception
    {
        public IllegalStateException() : base() { }
        public IllegalStateException(string message) : base(message) { }
    }
}

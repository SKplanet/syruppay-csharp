using System;

namespace SyrupPayToken.exception
{
    public class AlreadyBuiltException : Exception
    {
        public AlreadyBuiltException() : base() { }
        public AlreadyBuiltException(string message) : base(message) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SyrupPayToken.exception
{
    class InvalidTypeException : Exception
    {
        public InvalidTypeException() : base() {}
        public InvalidTypeException(string message) : base(message) { }
    }
}

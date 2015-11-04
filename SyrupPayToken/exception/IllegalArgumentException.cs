using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyrupPayToken.exception
{
    public class IllegalArgumentException : Exception
    {
        public IllegalArgumentException() : base() { }
        public IllegalArgumentException(string message) : base(message) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyrupPayToken.exception
{
    class JsonOperationException : Exception
    {
        public JsonOperationException() : base() { }
        public JsonOperationException(string message) : base(message) { }
    }
}

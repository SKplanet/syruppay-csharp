using System;

namespace SyrupPayJose
{
    class InvalidSignatureException : Exception
    {
        public InvalidSignatureException() : base() { }
        public InvalidSignatureException(String message) : base(message) { }
    }
}

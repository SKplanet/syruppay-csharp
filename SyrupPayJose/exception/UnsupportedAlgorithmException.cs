using System;

namespace SyrupPayJose
{
    class UnsupportedAlgorithmException : Exception
    {
        public UnsupportedAlgorithmException() : base() { }
        public UnsupportedAlgorithmException(String message) : base(message) { }
    }
}

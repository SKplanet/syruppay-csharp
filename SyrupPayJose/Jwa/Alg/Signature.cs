using System;

namespace SyrupPayJose.Jwa.Alg
{
    abstract class Signature : IJwsAlgorithm
    {
        private int keyLength;

        public Signature(int keyLength)
        {
            this.keyLength = keyLength;
        }

        abstract public byte[] Sign(byte[] key, byte[] src);
        abstract public void Verify(byte[] key, byte[] src, byte[] expected);

        private void IsValidKeyLength(byte[] key)
        {
            if (keyLength != key.Length)
            {
                throw new ArgumentException("Jws key must be " + keyLength + " bytes.");
            }
        }
    }
}

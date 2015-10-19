using SyrupPayJose.Jwa.Enc;
using System;

namespace SyrupPayJose.Jwa.Alg
{
    public abstract class AesKeyWrap : KeyEncryption
    {
        private int keyLength = 0;

        public AesKeyWrap(int keyLength)
        {
            this.keyLength = keyLength;
        }

        public override JwaAlgResult Encryption(byte[] key, ContentEncryptKeyGenerator cekGenerator)
        {
            IsValidKeyLength(key);
            byte[] src = cekGenerator.GenerateRandomKey();
            return Wrap(key, src);
        }

        public override byte[] Decryption(byte[] key, byte[] src)
        {
            IsValidKeyLength(key);
            return UnWrap(key, src);
        }

        private void IsValidKeyLength(byte[] key)
        {
            if (keyLength != key.Length)
            {
                throw new ArgumentException("Jwe key must be " + keyLength + " bytes.");
            }
        }

        abstract public JwaAlgResult Wrap(byte[] key, byte[] src);
        abstract public byte[] UnWrap(byte[] key, byte[] src);
    }
}

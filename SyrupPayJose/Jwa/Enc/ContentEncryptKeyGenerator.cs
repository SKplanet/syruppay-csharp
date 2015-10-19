using System.Security.Cryptography;

namespace SyrupPayJose.Jwa.Enc
{
    public class ContentEncryptKeyGenerator
    {
        private int keyLength;
        private byte[] cek;

        public ContentEncryptKeyGenerator(int keyLength)
        {
            this.keyLength = keyLength;
        }

        public byte[] UserEncryptionKey
        {
            set { this.cek = value; }
        }

        private int KeyBitLength
        {
            get { return keyLength * 8; }
        }

        public byte[] GenerateRandomKey()
        {
            if (cek == null)
            {
                var aes = new AesManaged();
                aes.KeySize = KeyBitLength;
                aes.GenerateKey();
                cek = aes.Key;
            }

            return cek;
        }
    }
}

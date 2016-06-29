using System.Security.Cryptography;

namespace SyrupPayJose.Jwa.Enc
{
    public struct JweEncResult
    {
        public byte[] cipherText;
        public byte[] at;
        public byte[] iv;
    }

    public abstract class ContentEncryption : IJweEncryption
    {
        protected int keyLength = 0;
        protected int ivLength = 0;

        public ContentEncryption(int keyLength, int ivLength)
        {
            this.keyLength = keyLength;
            this.ivLength = ivLength;
        }

        public int KeyLength
        {
            get { return keyLength; }
        }

        public int IvLength
        {
            get { return ivLength; }
        }

        public byte[] GenerateRandomIv()
        {
            using (Rijndael aes = Rijndael.Create())
            {
                aes.KeySize = ivLength * 8;
                aes.GenerateIV();
                return aes.IV;
            }
        }

        abstract public JweEncResult EncryptionAndSign(byte[] key, byte[] iv, byte[] src, byte[] aad);
        abstract public byte[] VerifyAndDecryption(byte[] key, byte[] iv, byte[] src, byte[] aad, byte[] expected);
        abstract public ContentEncryptKeyGenerator getContentEncryptionKeyGenerator();
    }
}

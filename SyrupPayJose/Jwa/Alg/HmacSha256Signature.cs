using System.Security.Cryptography;

namespace SyrupPayJose.Jwa.Alg
{
    class HmacSha256Signature : Signature
    {
        public HmacSha256Signature(int keyLength) : base(keyLength) { }

        public override byte[] Sign(byte[] key, byte[] src)
        {
            var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(src);
        }

        public override void Verify(byte[] key, byte[] src, byte[] expected)
        {
            byte[] actual = Sign(key, src);

            if (expected.Length != actual.Length)
            {
                throw new InvalidSignatureException("Invalid Signature");
            }

            for (int i = 0; i < actual.Length; i++)
            {
                if (expected[i] != actual[i])
                    throw new InvalidSignatureException("Invalid Signature");
            }
        }
    }
}

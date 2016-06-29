using System;
using System.IO;
using System.Security.Cryptography;

namespace SyrupPayJose.Jwa.Enc
{
    public class AesEncryptionWithHmacSha : ContentEncryption
    {
        public AesEncryptionWithHmacSha(int keyLength, int ivLength) : base(keyLength, ivLength) { }

        public override JweEncResult EncryptionAndSign(byte[] key, byte[] iv, byte[] src, byte[] aad)
        {
            iv = iv != null ? iv : GenerateRandomIv();

            var hmacKey = new byte[keyLength / 2];
            Buffer.BlockCopy(key, 0, hmacKey, 0, hmacKey.Length);

            var encKey = new byte[keyLength / 2];
            Buffer.BlockCopy(key, hmacKey.Length, encKey, 0, encKey.Length);

            byte[] b = Encryption(encKey, iv, src);
            byte[] s = Sign(hmacKey, iv, aad, b);

            JweEncResult jweEncResponse;
            jweEncResponse.cipherText = b;
            jweEncResponse.at = s;
            jweEncResponse.iv = iv;

            return jweEncResponse;
        }

        public byte[] Encryption(byte[] key, byte[] iv, byte[] src)
        {
            using (Rijndael aes = Rijndael.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(src, 0, src.Length);
                            cs.FlushFinalBlock();

                            return ms.ToArray();
                        }
                    }
                }
            }
        }

        public byte[] Sign(byte[] key, byte[] iv, byte[] aad, byte[] src)
        {
            byte[] b = MakeSignParts(iv, aad, src);

            var hmac = new HMACSHA256(key);
            var s = hmac.ComputeHash(b);
            var at = new byte[16];
            Buffer.BlockCopy(s, 0, at, 0, at.Length);

            return at;
        }

        public byte[] MakeSignParts(byte[] iv, byte[] aad, byte[] src)
        {
            byte[] al = GetAl(aad.Length);

            byte[] b = new byte[aad.Length + iv.Length + src.Length + al.Length];

            Buffer.BlockCopy(aad, 0, b, 0, aad.Length);
            Buffer.BlockCopy(iv, 0, b, aad.Length, iv.Length);
            Buffer.BlockCopy(src, 0, b, aad.Length + ivLength, src.Length);
            Buffer.BlockCopy(al, 0, b, aad.Length + ivLength + src.Length, al.Length);

            return b;
        }

        public byte[] GetAl(int len)
        {
            var aadlen = Convert.ToUInt64(len * 8);
            var b = BitConverter.GetBytes(aadlen);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }

        public override ContentEncryptKeyGenerator getContentEncryptionKeyGenerator()
        {
            return new ContentEncryptKeyGenerator(keyLength);
        }

        public override byte[] VerifyAndDecryption(byte[] key, byte[] iv, byte[] src, byte[] aad, byte[] expected)
        {
            var hmacKey = new byte[keyLength / 2];
            Buffer.BlockCopy(key, 0, hmacKey, 0, hmacKey.Length);

            var encKey = new byte[keyLength / 2];
            Buffer.BlockCopy(key, hmacKey.Length, encKey, 0, encKey.Length);

            Verify(hmacKey, iv, aad, src, expected);
            return Decryption(encKey, iv, src);
        }

        public void Verify(byte[] key, byte[] iv, byte[] aad, byte[] src, byte[] expected)
        {
            byte[] actual = Sign(key, iv, aad, src);
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

        public byte[] Decryption(byte[] key, byte[] iv, byte[] src)
        {
            using (Rijndael aes = Rijndael.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.IV = iv;
                aes.Key = key;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(src, 0, src.Length);
                            cs.FlushFinalBlock();

                            return ms.ToArray();
                        }
                    }
                }
            }
        }
    }
}

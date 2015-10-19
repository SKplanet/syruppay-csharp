using NUnit.Framework;
using SyrupPayJose.Jwa.Alg;
using SyrupPayJose.Jwa.Enc;
using SyrupPayJose.Utils;
using System;

namespace Test.Jwa.Alg
{
    [TestFixture]
    public class Aes256KeyWrapTest
    {
        [Test]
        public void TestEncryption()
        {
            var aes256KeyWrap = new Aes256KeyWrap();

            byte[] cek = { (byte) 4, (byte) 211, (byte) 31, (byte) 197, (byte) 84, (byte) 157, (byte) 252,
                (byte) 254, (byte) 11, (byte) 100, (byte) 157, (byte) 250, (byte) 63, (byte) 170, (byte) 106,
                (byte) 206, (byte) 107, (byte) 124, (byte) 212, (byte) 45, (byte) 111, (byte) 107, (byte) 9,
                (byte) 219, (byte) 200, (byte) 177, (byte) 0, (byte) 240, (byte) 143, (byte) 156, (byte) 44, (byte) 207 };

            byte[] key = StringUtils.StringToByte("12345678901234567890123456789012");

            ContentEncryptKeyGenerator cekGenerator = new ContentEncryptKeyGenerator(32);
            cekGenerator.UserEncryptionKey = cek;
            JwaAlgResult jwaAlgResult = aes256KeyWrap.Encryption(key, cekGenerator);
            byte[] b = jwaAlgResult.encryptedCek;

            Assert.AreEqual("UVf1x6nVsOmpxjlUFSiQdzbsOMYuAh3FQlH0nY3yhDWVJFh9HLtHIQ", Base64.base64urlencode(b));
        }

        [Test]
        public void TestDecryption()
        {
            byte[] k = { (byte) 4, (byte) 211, (byte) 31, (byte) 197, (byte) 84, (byte) 157, (byte) 252, (byte) 254,
                (byte) 11, (byte) 100, (byte) 157, (byte) 250, (byte) 63, (byte) 170, (byte) 106, (byte) 206,
                (byte) 107, (byte) 124, (byte) 212, (byte) 45, (byte) 111, (byte) 107, (byte) 9, (byte) 219,
                (byte) 200, (byte) 177, (byte) 0, (byte) 240, (byte) 143, (byte) 156, (byte) 44, (byte) 207 };
            string expected = Base64.base64urlencode(k);

            string cek = "UVf1x6nVsOmpxjlUFSiQdzbsOMYuAh3FQlH0nY3yhDWVJFh9HLtHIQ";
            byte[] key = StringUtils.StringToByte("12345678901234567890123456789012");

            var aes256KeyWrap = new Aes256KeyWrap();
            byte[] b = aes256KeyWrap.Decryption(key, Base64.base64urldecode(cek));

            Assert.AreEqual(expected, Base64.base64urlencode(b));

            Console.WriteLine("expected = " + expected);
            Console.WriteLine("Base64.base64urlencode(b) = " + Base64.base64urlencode(b));
        }
    }
}

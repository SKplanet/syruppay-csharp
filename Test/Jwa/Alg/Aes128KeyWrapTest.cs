using NUnit.Framework;
using SyrupPayJose.Jwa.Alg;
using SyrupPayJose.Utils;
using SyrupPayJose.Jwa.Enc;

namespace Test.Jwa.Alg
{
    [TestFixture]
    public class Aes128KeyWrapTest
    {
        [Test]
        public void TestEncryption()
        {
            var aes128KeyWrap = new Aes128KeyWrap(16);

            byte[] cek = { (byte) 4, (byte) 211, (byte) 31, (byte) 197, (byte) 84, (byte) 157, (byte) 252,
                (byte) 254, (byte) 11, (byte) 100, (byte) 157, (byte) 250, (byte) 63, (byte) 170, (byte) 106,
                (byte) 206, (byte) 107, (byte) 124, (byte) 212, (byte) 45, (byte) 111, (byte) 107, (byte) 9,
                (byte) 219, (byte) 200, (byte) 177, (byte) 0, (byte) 240, (byte) 143, (byte) 156, (byte) 44, (byte) 207 };

            byte[] key = Base64.base64urldecode("GawgguFyGrWKav7AX4VKUg");

            ContentEncryptKeyGenerator cekGenerator = new ContentEncryptKeyGenerator(32);
            cekGenerator.UserEncryptionKey = cek;
            JwaAlgResult jwaAlgResult = aes128KeyWrap.Encryption(key, cekGenerator);
            byte[] b = jwaAlgResult.encryptedCek;

            Assert.AreEqual("6KB707dM9YTIgHtLvtgWQ8mKwboJW3of9locizkDTHzBC2IlrT1oOQ", Base64.base64urlencode(b));
        }

        [Test]
        public void TestDecryption()
        {
            byte[] k = { (byte) 4, (byte) 211, (byte) 31, (byte) 197, (byte) 84, (byte) 157, (byte) 252, (byte) 254,
                (byte) 11, (byte) 100, (byte) 157, (byte) 250, (byte) 63, (byte) 170, (byte) 106, (byte) 206,
                (byte) 107, (byte) 124, (byte) 212, (byte) 45, (byte) 111, (byte) 107, (byte) 9, (byte) 219,
                (byte) 200, (byte) 177, (byte) 0, (byte) 240, (byte) 143, (byte) 156, (byte) 44, (byte) 207 };
            string expected = Base64.base64urlencode(k);

            string cek = "6KB707dM9YTIgHtLvtgWQ8mKwboJW3of9locizkDTHzBC2IlrT1oOQ";
            byte[] key = Base64.base64urldecode("GawgguFyGrWKav7AX4VKUg");

            var aes128KeyWrap = new Aes128KeyWrap(16);
            byte[] b = aes128KeyWrap.Decryption(key, Base64.base64urldecode(cek));

            Assert.AreEqual(expected, Base64.base64urlencode(b));
        }
    }
}

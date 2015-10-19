using System;
using NUnit.Framework;
using SyrupPayJose;
using SyrupPayJose.Utils;
using SyrupPayJose.Jwa.Enc;
using SyrupPayJose.Jwa;

namespace Test.Jwa.Enc
{
    [TestFixture]
    public class Aes128HmacEncryptionTest
    {
        [Test]
        public void TestEncryption()
        {
            byte[] src = { (byte) 76, (byte) 105, (byte) 118, (byte) 101, (byte) 32, (byte) 108, (byte) 111, (byte) 110,
                (byte) 103, (byte) 32, (byte) 97, (byte) 110, (byte) 100, (byte) 32, (byte) 112, (byte) 114,
                (byte) 111, (byte) 115, (byte) 112, (byte) 101, (byte) 114, (byte) 46 };

            byte[] key = { (byte) 4, (byte) 211, (byte) 31, (byte) 197, (byte) 84, (byte) 157, (byte) 252, (byte) 254,
                (byte) 11, (byte) 100, (byte) 157, (byte) 250, (byte) 63, (byte) 170, (byte) 106, (byte) 206,
                (byte) 107, (byte) 124, (byte) 212, (byte) 45, (byte) 111, (byte) 107, (byte) 9, (byte) 219,
                (byte) 200, (byte) 177, (byte) 0, (byte) 240, (byte) 143, (byte) 156, (byte) 44, (byte) 207 };

            byte[] iv = { (byte) 3, (byte) 22, (byte) 60, (byte) 12, (byte) 43, (byte) 67, (byte) 104, (byte) 105,
                (byte) 108, (byte) 108, (byte) 105, (byte) 99, (byte) 111, (byte) 116, (byte) 104, (byte) 101 };

            JoseHeader header = new JoseHeader(JsonWebAlgorithm.A128KW, JsonWebAlgorithm.A128CBC_HS256);
            byte[] aad = StringUtils.StringToByte(header.GetSerialize());

            var obj = new AesEncryptionWithHmacSha(32, 16);
            var jweEncResult = obj.EncryptionAndSign(key, iv, src, aad);

            Assert.AreEqual("KDlTtXchhZTGufMYmOYGS4HffxPSUrfmqCHXaI9wOGY", Base64.base64urlencode(jweEncResult.cipherText));
            Assert.AreEqual("U0m_YmjN04DJvceFICbCVQ", Base64.base64urlencode(jweEncResult.at));
        }

        [Test]
        public void TestAl()
        {
            int[] expected = { 0, 0, 0, 0, 0, 0, 1, 152 };

            JoseHeader header = new JoseHeader(JsonWebAlgorithm.A128KW, JsonWebAlgorithm.A128CBC_HS256);
            byte[] aad = StringUtils.StringToByte(header.GetSerialize());

            var obj = new AesEncryptionWithHmacSha(32, 16);
            byte[] actual = obj.GetAl(aad.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], (int)actual[i] & 0xFF);
            }
        }

        [Test]
        public void TestMakeSignParts()
        {
            byte[] key = { (byte) 4, (byte) 211, (byte) 31, (byte) 197, (byte) 84, (byte) 157, (byte) 252, (byte) 254,
                (byte) 11, (byte) 100, (byte) 157, (byte) 250, (byte) 63, (byte) 170, (byte) 106, (byte) 206 };

            byte[] iv = { (byte) 3, (byte) 22, (byte) 60, (byte) 12, (byte) 43, (byte) 67, (byte) 104, (byte) 105,
                (byte) 108, (byte) 108, (byte) 105, (byte) 99, (byte) 111, (byte) 116, (byte) 104, (byte) 101 };

            byte[] src = { (byte) 40, (byte) 57, (byte) 83, (byte) 181, (byte) 119, (byte) 33, (byte) 133,
                (byte) 148, (byte) 198, (byte) 185, (byte) 243, (byte) 24, (byte) 152, (byte) 230, (byte) 6, (byte) 75,
                (byte) 129, (byte) 223, (byte) 127, (byte) 19, (byte) 210, (byte) 82, (byte) 183, (byte) 230,
                (byte) 168, (byte) 33, (byte) 215, (byte) 104, (byte) 143, (byte) 112, (byte) 56, (byte) 102 };

            byte[] expected = { (byte) 101, (byte) 121, (byte) 74, (byte) 104, (byte) 98, (byte) 71, (byte) 99, (byte) 105,
                (byte) 79, (byte) 105, (byte) 74, (byte) 66, (byte) 77, (byte) 84, (byte) 73, (byte) 52, (byte) 83,
                (byte) 49, (byte) 99, (byte) 105, (byte) 76, (byte) 67, (byte) 74, (byte) 108, (byte) 98, (byte) 109,
                (byte) 77, (byte) 105, (byte) 79, (byte) 105, (byte) 74, (byte) 66, (byte) 77, (byte) 84, (byte) 73,
                (byte) 52, (byte) 81, (byte) 48, (byte) 74, (byte) 68, (byte) 76, (byte) 85, (byte) 104, (byte) 84,
                (byte) 77, (byte) 106, (byte) 85, (byte) 50, (byte) 73, (byte) 110, (byte) 48, (byte) 3, (byte) 22,
                (byte) 60, (byte) 12, (byte) 43, (byte) 67, (byte) 104, (byte) 105, (byte) 108, (byte) 108, (byte) 105,
                (byte) 99, (byte) 111, (byte) 116, (byte) 104, (byte) 101, (byte) 40, (byte) 57, (byte) 83, (byte) 181,
                (byte) 119, (byte) 33, (byte) 133, (byte) 148, (byte) 198, (byte) 185, (byte) 243, (byte) 24,
                (byte) 152, (byte) 230, (byte) 6, (byte) 75, (byte) 129, (byte) 223, (byte) 127, (byte) 19, (byte) 210,
                (byte) 82, (byte) 183, (byte) 230, (byte) 168, (byte) 33, (byte) 215, (byte) 104, (byte) 143,
                (byte) 112, (byte) 56, (byte) 102, (byte) 0, (byte) 0, (byte) 0, (byte) 0, (byte) 0, (byte) 0,
                (byte) 1, (byte) 152 };

            JoseHeader header = new JoseHeader(JsonWebAlgorithm.A128KW, JsonWebAlgorithm.A128CBC_HS256);
            byte[] aad = StringUtils.StringToByte(header.GetSerialize());

            var obj = new AesEncryptionWithHmacSha(32, 16);
            var actual = obj.MakeSignParts(iv, aad, src);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], (int)actual[i] & 0xFF);
            }
        }

        [Test]
        public void TestAt()
        {
            byte[] hmacParts = { (byte) 101, (byte) 121, (byte) 74, (byte) 104, (byte) 98, (byte) 71, (byte) 99, (byte) 105,
                (byte) 79, (byte) 105, (byte) 74, (byte) 66, (byte) 77, (byte) 84, (byte) 73, (byte) 52, (byte) 83,
                (byte) 49, (byte) 99, (byte) 105, (byte) 76, (byte) 67, (byte) 74, (byte) 108, (byte) 98, (byte) 109,
                (byte) 77, (byte) 105, (byte) 79, (byte) 105, (byte) 74, (byte) 66, (byte) 77, (byte) 84, (byte) 73,
                (byte) 52, (byte) 81, (byte) 48, (byte) 74, (byte) 68, (byte) 76, (byte) 85, (byte) 104, (byte) 84,
                (byte) 77, (byte) 106, (byte) 85, (byte) 50, (byte) 73, (byte) 110, (byte) 48, (byte) 3, (byte) 22,
                (byte) 60, (byte) 12, (byte) 43, (byte) 67, (byte) 104, (byte) 105, (byte) 108, (byte) 108, (byte) 105,
                (byte) 99, (byte) 111, (byte) 116, (byte) 104, (byte) 101, (byte) 40, (byte) 57, (byte) 83, (byte) 181,
                (byte) 119, (byte) 33, (byte) 133, (byte) 148, (byte) 198, (byte) 185, (byte) 243, (byte) 24,
                (byte) 152, (byte) 230, (byte) 6, (byte) 75, (byte) 129, (byte) 223, (byte) 127, (byte) 19, (byte) 210,
                (byte) 82, (byte) 183, (byte) 230, (byte) 168, (byte) 33, (byte) 215, (byte) 104, (byte) 143,
                (byte) 112, (byte) 56, (byte) 102, (byte) 0, (byte) 0, (byte) 0, (byte) 0, (byte) 0, (byte) 0,
                (byte) 1, (byte) 152 };

            byte[] secret = { (byte) 4, (byte) 211, (byte) 31, (byte) 197, (byte) 84, (byte) 157, (byte) 252, (byte) 254,
                (byte) 11, (byte) 100, (byte) 157, (byte) 250, (byte) 63, (byte) 170, (byte) 106, (byte) 206,
                (byte) 107, (byte) 124, (byte) 212, (byte) 45, (byte) 111, (byte) 107, (byte) 9, (byte) 219,
                (byte) 200, (byte) 177, (byte) 0, (byte) 240, (byte) 143, (byte) 156, (byte) 44, (byte) 207 };

            byte[] iv = { (byte) 3, (byte) 22, (byte) 60, (byte) 12, (byte) 43, (byte) 67, (byte) 104, (byte) 105,
                (byte) 108, (byte) 108, (byte) 105, (byte) 99, (byte) 111, (byte) 116, (byte) 104, (byte) 101 };

            byte[] aad = { (byte) 101, (byte) 121, (byte) 74, (byte) 104, (byte) 98, (byte) 71, (byte) 99, (byte) 105,
                (byte) 79, (byte) 105, (byte) 74, (byte) 66, (byte) 77, (byte) 84, (byte) 73, (byte) 52, (byte) 83,
                (byte) 49, (byte) 99, (byte) 105, (byte) 76, (byte) 67, (byte) 74, (byte) 108, (byte) 98, (byte) 109,
                (byte) 77, (byte) 105, (byte) 79, (byte) 105, (byte) 74, (byte) 66, (byte) 77, (byte) 84, (byte) 73,
                (byte) 52, (byte) 81, (byte) 48, (byte) 74, (byte) 68, (byte) 76, (byte) 85, (byte) 104, (byte) 84,
                (byte) 77, (byte) 106, (byte) 85, (byte) 50, (byte) 73, (byte) 110, (byte) 48 };

            byte[] cipherText = { (byte) 40, (byte) 57, (byte) 83, (byte) 181, (byte) 119, (byte) 33, (byte) 133,
                (byte) 148, (byte) 198, (byte) 185, (byte) 243, (byte) 24, (byte) 152, (byte) 230, (byte) 6, (byte) 75,
                (byte) 129, (byte) 223, (byte) 127, (byte) 19, (byte) 210, (byte) 82, (byte) 183, (byte) 230,
                (byte) 168, (byte) 33, (byte) 215, (byte) 104, (byte) 143, (byte) 112, (byte) 56, (byte) 102 };

            byte[] key = new byte[16];
            Buffer.BlockCopy(secret, 0, key, 0, 16);

            var obj = new AesEncryptionWithHmacSha(32, 16);
            var at = obj.Sign(key, iv, aad, cipherText);
            Assert.AreEqual("U0m_YmjN04DJvceFICbCVQ", Base64.base64urlencode(at));
        }
    }
}

using System;
using NUnit.Framework;
using SyrupPayJose;
using SyrupPayJose.Jwa;

namespace Test
{
    [TestFixture]
    public class JoseTest
    {
        [Test]
        public void TestMethod1()
        {
            var payload = "Test";
            var kid = "Sample";
            var key = "1234567890123456";

            var jweToken = new Jose().Configuration(
                JoseBuilders.JsonEncryptionCompactSerializationBuilder()
                    .Header(new JoseHeader(JsonWebAlgorithm.A128KW, JsonWebAlgorithm.A128CBC_HS256, kid))
                    .Payload(payload)
                    .Key(key)
                ).Serialization();

            Console.WriteLine("JWE = " + jweToken);

            var serializeAction = new Jose().Configuration(
                JoseBuilders.JsonEncryptionCompactDeserializationBuilder()
                    .SerializedSource(jweToken)
                    .Key(key)
                );

            var result = serializeAction.Deserialization();
            var header = serializeAction.GetHeader();

            Assert.AreEqual(payload, result);
            Assert.AreEqual(kid, header.Kid);
        }

        [Test]
        public void TestMethod2()
        {
            var payload = "Test";
            var kid = "Sample";
            var key = "1234567890123456";

            var jwsToken = new Jose().Configuration(
                JoseBuilders.JsonSignatureCompactSerializationBuilder()
                    .Header(new JoseHeader(JsonWebAlgorithm.HS256, JsonWebAlgorithm.NONE, kid))
                    .Payload(payload)
                    .Key(key)
                ).Serialization();

            var serializeAction = new Jose().Configuration(
                JoseBuilders.JsonSignatureCompactDeserializationBuilder()
                    .SerializedSource(jwsToken)
                    .Key(key)
                );

            var result = serializeAction.Deserialization();
            var header = serializeAction.GetHeader();

            Assert.AreEqual(payload, result);
            Assert.AreEqual(kid, header.Kid);
        }
    }
}

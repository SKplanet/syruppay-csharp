using NUnit.Framework;
using SyrupPayJose;
using SyrupPayJose.Jwa;
using SyrupPayJose.Jwe;

namespace Test.Jwe
{
    [TestFixture]
    public class JweSerializerTest
    {
        [Test]
        public void TestSerializer()
        {
            var payload = "apple";
            var kid = "sample";
            var key = "1234567890123456";

            var header = new JoseHeader(JsonWebAlgorithm.A128KW, JsonWebAlgorithm.A128CBC_HS256, kid);
            var serializer = new JweSerializer(header, payload, key);
            var jweToken = serializer.CompactSerialization();

            var deserializer = new JweSerializer(jweToken, key);
            var actual = deserializer.CompactDeserialization();

            Assert.AreEqual(payload, actual);

            header = deserializer.GetHeader();
            Assert.AreEqual(kid, header.Kid);
        }
    }
}

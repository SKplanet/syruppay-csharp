using NUnit.Framework;
using SyrupPayJose;
using SyrupPayJose.Jwa;
using SyrupPayJose.Jws;

namespace Test.Jws
{
    [TestFixture]
    public class JwsSerializerTest
    {
        [Test]
        public void TestSerializer()
        {
            var expected = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.YXBwbGU.9CofnzzDvqCYT_tuMqaBXAgX9yZSsbTMEAsz5EolTU4";
            var payload = "apple";
            var key = "1234567890123456";

            var joseHeader = new JoseHeader(JsonWebAlgorithm.HS256);
            joseHeader.SetHeader(JoseHeaderSpec.TYP, "JWT");

            var serializer = new JwsSerializer(joseHeader, payload, key);
            var jwsToken = serializer.CompactSerialization();

            Assert.AreEqual(expected, jwsToken);

            serializer = new JwsSerializer(jwsToken, key);
            var actual = serializer.CompactDeserialization();

            Assert.AreEqual(payload, actual);
        }
    }
}

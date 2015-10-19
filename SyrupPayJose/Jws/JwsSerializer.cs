using SyrupPayJose.Jwa;
using SyrupPayJose.Utils;
using System;

namespace SyrupPayJose.Jws
{
    struct JwsStructure
    {
        public JoseHeader joseHeader;
        public byte[] payload;
        public byte[] signature;

        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}",
                joseHeader.GetSerialize(),
                Base64.base64urlencode(payload),
                Base64.base64urlencode(signature));
        }

        public byte[] SignSource
        {
            get
            {
                return StringUtils.StringToByte(String.Format("{0}.{1}",
                    joseHeader.GetSerialize(),
                    Base64.base64urlencode(payload)));
            }
        }

        public void Set(string src)
        {
            var token = src.Split('.');
            if (token == null || token.Length != 3)
            {
                throw new IllegalEncryptionTokenException();
            }

            joseHeader = new JoseHeader();
            joseHeader.SetSerialize(token[0]);
            payload = Base64.base64urldecode(token[1]);
            signature = Base64.base64urldecode(token[2]);
        }
    }

    public class JwsSerializer : IJoseAction
    {
        private byte[] key;

        JwsStructure jwsStructure;

        public JwsSerializer(JoseHeader header, string payload, string key)
        {
            jwsStructure.joseHeader = header;
            jwsStructure.payload = StringUtils.StringToByte(payload);
            this.key = StringUtils.StringToByte(key);
        }

        public JwsSerializer(String serialized, string key)
        {
            this.jwsStructure.Set(serialized);
            this.key = StringUtils.StringToByte(key);
        }

        public JoseHeader GetHeader()
        {
            return jwsStructure.joseHeader;
        }

        public string CompactSerialization()
        {
            IJwsAlgorithm jwsAlgorithm = JwaFactory.GetJwsAlgorithm(jwsStructure.joseHeader.Alg);
            byte[] b = jwsAlgorithm.Sign(key, jwsStructure.SignSource);
            jwsStructure.signature = b;

            return jwsStructure.ToString();
        }

        public string CompactDeserialization()
        {
            IJwsAlgorithm jwsAlgorithm = JwaFactory.GetJwsAlgorithm(jwsStructure.joseHeader.Alg);
            jwsAlgorithm.Verify(key,
                jwsStructure.SignSource,
                jwsStructure.signature);

            return StringUtils.ByteToString(jwsStructure.payload);
        }
    }
}

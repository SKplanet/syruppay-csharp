using SyrupPayJose.Jwe;
using SyrupPayJose.Jws;
using System;

namespace SyrupPayJose
{
    public class SerializationBuilder : JoseCompactBuilder
    {
        private JoseHeader header;
        private string payload;

        public override IJoseAction Create()
        {
            switch (joseSerializeType)
            {
                case JoseSerializeType.COMPACT_SERIALIZATION:
                    if (JoseMethod.JWE == joseMethod && JoseActionType.SERIALIZATION == joseActionType)
                    {
                        return new JweSerializer(header, payload, key);
                    }
                    else if (JoseMethod.JWS == joseMethod && JoseActionType.SERIALIZATION == joseActionType)
                    {
                        return new JwsSerializer(header, payload, key);
                    }
                    else
                    {
                        throw new ArgumentException("unknown JoseSerializeType and JoseActionType");
                    }
                case JoseSerializeType.JSON_SERIALIZATION:
                    return null;
                default:
                    return null;
            }
        }

        public SerializationBuilder(JoseMethod joseMethod, JoseActionType joseActionType)
        {
            base.CompactBuilder(joseMethod, joseActionType);
            header = new JoseHeader();
        }

        public SerializationBuilder Header(JoseHeader header)
        {
            this.header = header;
            return this;
        }

        public SerializationBuilder Payload(string payload)
        {
            this.payload = payload;
            return this;
        }
    }
}

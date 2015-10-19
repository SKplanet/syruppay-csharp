using SyrupPayJose.Jwe;
using SyrupPayJose.Jws;
using System;

namespace SyrupPayJose
{
    public class DeserializationBuilder : JoseCompactBuilder
    {
        private string serializedSource;

        public override IJoseAction Create()
        {
            switch (joseSerializeType)
            {
                case JoseSerializeType.COMPACT_SERIALIZATION:
                    if (JoseMethod.JWE == joseMethod && JoseActionType.DESERIALIZATION == joseActionType)
                    {
                        return new JweSerializer(serializedSource, key);
                    }
                    else if (JoseMethod.JWS == joseMethod && JoseActionType.DESERIALIZATION == joseActionType)
                    {
                        return new JwsSerializer(serializedSource, key);
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

        public DeserializationBuilder(JoseMethod joseMethod, JoseActionType joseActionType)
        {
            base.CompactBuilder(joseMethod, joseActionType);
        }

        public DeserializationBuilder SerializedSource(string serializedSource)
        {
            this.serializedSource = serializedSource;
            return this;
        }
    }
}

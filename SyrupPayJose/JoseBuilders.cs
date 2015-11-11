using System;

namespace SyrupPayJose
{
    public class JoseBuilders
    {
        public static SerializationBuilder JsonSignatureCompactSerializationBuilder()
        {
            return new SerializationBuilder(JoseMethod.JWS, JoseActionType.SERIALIZATION);
        }

        [Obsolete("use 'CompactDeserializationBuilder'", true)]
        public static DeserializationBuilder JsonSignatureCompactDeserializationBuilder()
        {
            return new DeserializationBuilder(JoseMethod.JWS, JoseActionType.DESERIALIZATION);
        }

        public static SerializationBuilder JsonEncryptionCompactSerializationBuilder()
        {
            return new SerializationBuilder(JoseMethod.JWE, JoseActionType.SERIALIZATION);
        }

        [Obsolete("use 'CompactDeserializationBuilder'", true)]
        public static DeserializationBuilder JsonEncryptionCompactDeserializationBuilder()
        {
            return new DeserializationBuilder(JoseMethod.JWE, JoseActionType.DESERIALIZATION);
        }

        public static DeserializationBuilder CompactDeserializationBuilder()
        {
            return new DeserializationBuilder(JoseActionType.DESERIALIZATION);
        }
    }
}

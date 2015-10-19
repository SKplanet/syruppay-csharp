namespace SyrupPayJose
{
    public class JoseBuilders
    {
        public static SerializationBuilder JsonSignatureCompactSerializationBuilder()
        {
            return new SerializationBuilder(JoseMethod.JWS, JoseActionType.SERIALIZATION);
        }

        public static DeserializationBuilder JsonSignatureCompactDeserializationBuilder()
        {
            return new DeserializationBuilder(JoseMethod.JWS, JoseActionType.DESERIALIZATION);
        }

        public static SerializationBuilder JsonEncryptionCompactSerializationBuilder()
        {
            return new SerializationBuilder(JoseMethod.JWE, JoseActionType.SERIALIZATION);
        }

        public static DeserializationBuilder JsonEncryptionCompactDeserializationBuilder()
        {
            return new DeserializationBuilder(JoseMethod.JWE, JoseActionType.DESERIALIZATION);
        }
    }
}

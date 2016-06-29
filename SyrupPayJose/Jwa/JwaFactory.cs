using SyrupPayJose.Utils;
using SyrupPayJose.Jwa.Alg;
using SyrupPayJose.Jwa.Enc;

namespace SyrupPayJose.Jwa
{
    class JwaFactory
    {
        public static IJweAlgorithm GetJweAlgorithm(string alg)
        {
            switch (EnumString.GetEnum(alg))
            {
                case JsonWebAlgorithm.A128KW:
                    return new Aes128KeyWrap();
                case JsonWebAlgorithm.A256KW:
                    return new Aes256KeyWrap();
                default:
                    throw new UnsupportedAlgorithmException("Unknown Jwe Algorithm");
            }
        }

        public static IJweEncryption GetJweEncryption(string enc)
        {
            switch (EnumString.GetEnum(enc))
            {
                case JsonWebAlgorithm.A128CBC_HS256:
                    return new AesEncryptionWithHmacSha(32, 16);
                default:
                    throw new UnsupportedAlgorithmException("Unknown Jwe Encryption Algorithm");
            }
        }

        public static IJwsAlgorithm GetJwsAlgorithm(string alg)
        {
            switch (EnumString.GetEnum(alg))
            {
                case JsonWebAlgorithm.HS256:
                    return new HmacSha256Signature(32);
                default:
                    throw new UnsupportedAlgorithmException("Unknown Jws Signature Algorithm");
            }
        }

        public static JoseMethod GetAlgorithmType(string alg)
        {
            switch (EnumString.GetEnum(alg))
            {
                case JsonWebAlgorithm.A128KW:
                    return JoseMethod.JWE;
                case JsonWebAlgorithm.A256KW:
                    return JoseMethod.JWE;
                case JsonWebAlgorithm.HS256:
                    return JoseMethod.JWS;
                default:
                    throw new UnsupportedAlgorithmException(alg+" is not supported");
            }
        }
    }
}
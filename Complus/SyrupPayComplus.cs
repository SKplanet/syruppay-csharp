using SyrupPayJose;
using SyrupPayToken;
using SyrupPayToken.Claims;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Runtime.InteropServices;

[assembly: ApplicationActivation(ActivationOption.Server)]
[assembly: ApplicationAccessControl(false)]
namespace SyrupPay
{
    public class JoseSerializer : ServicedComponent
    {
        public string toJwe(string iss, string alg, string enc, string key, string payload)
        {
            JoseHeader header = new JoseHeader();
            header.SetHeader(JoseHeaderSpec.ALG, alg);
            header.SetHeader(JoseHeaderSpec.ENC, enc);
            header.SetHeader(JoseHeaderSpec.KID, iss);

            return new Jose().Configuration(
                JoseBuilders.JsonEncryptionCompactSerializationBuilder()
                    .Header(header)
                    .Payload(payload)
                    .Key(key)
                ).Serialization();
        }

        public string toJws(string iss, string alg, string key, string payload)
        {
            JoseHeader header = new JoseHeader();
            header.SetHeader(JoseHeaderSpec.ALG, alg);
            header.SetHeader(JoseHeaderSpec.KID, iss);

            return new Jose().Configuration(
                JoseBuilders.JsonSignatureCompactSerializationBuilder()
                    .Header(header)
                    .Payload(payload)
                    .Key(key)
                ).Serialization();
        }
    }

    public class JoseDeserializer : ServicedComponent
    {
        public string fromJose(string key, string src)
        {
            return new Jose().Configuration(
                JoseBuilders.CompactDeserializationBuilder()
                    .SerializedSource(src)
                    .Key(key)
                ).Deserialization();
        }
    }

    public 
}

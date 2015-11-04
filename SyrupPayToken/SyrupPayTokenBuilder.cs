using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SyrupPayJose;
using SyrupPayJose.Jwa;
using SyrupPayJose.Utils;
using SyrupPayToken.Claims;
using SyrupPayToken.exception;
using SyrupPayToken.jwt;
using System;
using System.Diagnostics;
using System.Globalization;

namespace SyrupPayToken
{
    public class SyrupPayTokenBuilder : AbstractConfiguredTokenBuilder<Jwt, SyrupPayTokenBuilder>, IClaimBuilder<Jwt>, ITokenBuilder<SyrupPayTokenBuilder>
    {
        private String iss;
        private long nbf;
        private String sub;
        private int expiredMinutes = 10;

        public static IToken<SyrupPayTokenBuilder> Verify(String token, String key)
        {
            var payload = new Jose().Configuration(
                JoseBuilders.JsonSignatureCompactDeserializationBuilder()
                    .SerializedSource(token)
                    .Key(key)
                ).Deserialization();

            try
            {
                MemoryTraceWriter traceWriter = new MemoryTraceWriter();
                var deserializeObject = JsonConvert.DeserializeObject<jwt.SyrupPayToken<SyrupPayTokenBuilder>>(payload, new JsonSerializerSettings
                {
                    TraceWriter = traceWriter,
                    MissingMemberHandling = MissingMemberHandling.Error,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                Debug.WriteLine(traceWriter.ToString());

                return deserializeObject;

            }
            catch (JsonSerializationException e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        public static IToken<SyrupPayTokenBuilder> Verify(String token, byte[] key)
        {
            return Verify(token, StringUtils.ByteToString(key));
        }

        public SyrupPayTokenBuilder Of(String merchantId)
        {
            this.iss = merchantId;
            return this;
        }

        public SyrupPayTokenBuilder AdditionalSubject(String subject)
        {
            this.sub = subject;
            return this;
        }

        public SyrupPayTokenBuilder IsNotValidBefore(long milliseconds)
        {
            this.nbf = milliseconds / 1000;
            return this;
        }

        public SyrupPayTokenBuilder IsNotValidBefore(String datetime, String format)
        {
            DateTime to = DateTime.ParseExact(datetime, format, CultureInfo.CurrentCulture);
            DateTime from = new DateTime(1970, 1, 1);
            this.nbf = (long)(to - from).TotalSeconds;
            return this;
        }

        public SyrupPayTokenBuilder IsNotValidBefore(DateTime datetime)
        {
            DateTime to = datetime;
            DateTime from = new DateTime(1970, 1, 1);
            this.nbf = (long)(to - from).TotalSeconds;
            return this;
        }

        public SyrupPayTokenBuilder ExpiredMinutes(int expiredMinutes)
        {
            this.expiredMinutes = expiredMinutes;
            return this;
        }

        public MerchantUserConfigurer<SyrupPayTokenBuilder> Login()
        {
            return GetOrApply(new MerchantUserConfigurer<SyrupPayTokenBuilder>());
        }

        public MerchantUserConfigurer<SyrupPayTokenBuilder> SignUp()
        {
            return GetOrApply(new MerchantUserConfigurer<SyrupPayTokenBuilder>());
        }

        public PayConfigurer<SyrupPayTokenBuilder> Pay()
        {
            return GetOrApply(new PayConfigurer<SyrupPayTokenBuilder>());
        }

        public OrderConfigurer<SyrupPayTokenBuilder> Checkout()
        {
            return GetOrApply(new OrderConfigurer<SyrupPayTokenBuilder>());
        }

        public MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder> MapToSyrupPayUser()
        {
            return GetOrApply(new MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder>());
        }

        public MapToSktUserConfigurer<SyrupPayTokenBuilder> MapToSktUser()
        {
            return GetOrApply(new MapToSktUserConfigurer<SyrupPayTokenBuilder>());
        }

        private C GetOrApply<C>(C configurer) where C : ClaimConfigurerAdapter<Jwt, SyrupPayTokenBuilder>
        {
            C existingConfig = GetConfigurer<C>(configurer.GetType());
            if (!Object.ReferenceEquals(null, existingConfig))
            {
                return existingConfig;
            }
            return Apply(configurer);
        }

        protected override Jwt DoBuild()
        {
            if (String.IsNullOrEmpty(iss))
            {
                throw new IllegalArgumentException("issuer couldn't be null. you should set of by SyrupPayTokenBuilder#of(String of)");
            }
            Jwt c = new Jwt();
            c.Iss = iss;
            c.Iat = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            c.Exp = c.Iat + (expiredMinutes * 60);
            c.Nbf = nbf;
            c.Sub = sub;
            return c;
        }

        public String GenerateTokenBy(String secret)
        {
            var h = new JoseHeader(JsonWebAlgorithm.HS256, JsonWebAlgorithm.NONE, iss);
            h.SetHeader(JoseHeaderSpec.TYP, "JWT");

            return new Jose().Configuration(
                JoseBuilders.JsonSignatureCompactSerializationBuilder()
                    .Header(h)
                    .Payload(ToJson())
                    .Key(secret)
                ).Serialization();
        }

        public String GenerateTokenBy(byte[] secret)
        {
            return GenerateTokenBy(StringUtils.ByteToString(secret));
        }

        protected String ToJson()
        {
            JObject o = (JObject)JToken.FromObject(Build());

            foreach (Type type in GetClasses())
            {
                IClaimConfigurer<Jwt, SyrupPayTokenBuilder> configurer = GetConfigurer<IClaimConfigurer<Jwt, SyrupPayTokenBuilder>>(type);
                configurer.ValidRequired();
                o.Add(configurer.ClaimName(), (JObject)JToken.FromObject(configurer));
            }

            try
            {
                MemoryTraceWriter traceWriter = new MemoryTraceWriter();
                var json = JsonConvert.SerializeObject(o, new JsonSerializerSettings
                {
                    TraceWriter = traceWriter,
                    MissingMemberHandling = MissingMemberHandling.Error,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                Debug.WriteLine(traceWriter.ToString());
                return json;
            }
            catch (JsonSerializationException e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }
    }
}

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

    /// <summary>
    /// Syrup Pay 에서 사용하는 토큰을 생성 및 암/복호화에 대한 기능을 수행한다.
    /// 토큰은 JWT 규격을 준수하며 Claim 에 대한 확장은 <see cref="IClaimConfigurer{O, B}"/>를 이용하여 확장할 수 있으며
    /// 이에 대한 인터페이스는 <see cref="SyrupPayTokenBuilder"/>를 통해 <see cref="Pay"/>와 <see cref="Login"/>와 같이 노출해야 한다.
    /// 
    /// </summary>
    public class SyrupPayTokenBuilder : AbstractConfiguredTokenBuilder<Jwt, SyrupPayTokenBuilder>, IClaimBuilder<Jwt>, ITokenBuilder<SyrupPayTokenBuilder>
    {
        private String iss;
        private long nbf;
        private String sub;
        private int expiredMinutes = 10;

        /// <summary>
        /// JWT 토큰에 대한 무결성을 검증(JWS)한 후 <see cref="IToken{H}"/> 객체를 생성하여 반환한다.
        /// </summary>
        /// <param name="token">문자열의 JWT</param>
        /// <param name="key">토큰 무결성을 검증할 키</param>
        /// <returns><seealso cref="SyrupPayToken{H}"/></returns>
        /// <exception cref="SyrupPayJose.InvalidSignatureException">JOSE JWS 검증 실패</exception>
        /// <exception cref="SyrupPayJose.UnsupportedAlgorithmException">지원하지 않은 Signature 알고리즘</exception>
        /// <exception cref="JsonOperationException">Json deserialize 처리 중 오류 발생</exception>"
        public static IToken<SyrupPayTokenBuilder> Verify(String token, String key)
        {
            var payload = new Jose().Configuration(
                JoseBuilders.CompactDeserializationBuilder()
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

                //Debug.WriteLine(traceWriter.ToString());

                return deserializeObject;

            }
            catch (JsonSerializationException e)
            {
                throw new JsonOperationException(e.Message);
            }
        }

        /// <summary>
        /// JWT 토큰에 대한 무결성을 검증(JWS)한 후 <see cref="IToken{H}"/> 객체를 생성하여 반환한다.
        /// </summary>
        /// <param name="token">문자열의 JWT</param>
        /// <param name="key">토큰 무결성을 검증할 키</param>
        /// <returns><seealso cref="SyrupPayToken{H}"/></returns>
        /// <exception cref="SyrupPayJose.InvalidSignatureException">JOSE JWS 검증 실패</exception>
        /// <exception cref="SyrupPayJose.UnsupportedAlgorithmException">지원하지 않은 Signature 알고리즘</exception>
        /// <exception cref="JsonOperationException">Json deserialize 처리 중 오류 발생</exception>"
        public static IToken<SyrupPayTokenBuilder> Verify(String token, byte[] key)
        {
            return Verify(token, StringUtils.ByteToString(key));
        }

        /// <summary>
        /// 시럽페이 토큰 생성 주체를 설정한다.
        /// </summary>
        /// <param name="merchantId">가맹점 ID</param>
        /// <returns>this</returns>
        public SyrupPayTokenBuilder Of(String merchantId)
        {
            this.iss = merchantId;
            return this;
        }

        /// <summary>
        /// JWT 의 sub 에 해당하는 서브 주제를 설정한다.
        /// </summary>
        /// <param name="subject">the subject</param>
        /// <returns>this</returns>
        public SyrupPayTokenBuilder AdditionalSubject(String subject)
        {
            this.sub = subject;
            return this;
        }

        /// <summary>
        /// JWT 의 nbf 에 해당하는 값으로 토큰이 유효한 시작 시간을 설정한다.
        /// </summary>
        /// <param name="milliseconds">밀리세컨드</param>
        /// <returns>this</returns>
        public SyrupPayTokenBuilder IsNotValidBefore(long milliseconds)
        {
            this.nbf = milliseconds / 1000;
            return this;
        }

        /// <summary>
        /// JWT 의 nbf 에 해당하는 값으로 토큰이 유효한 시작 시간을 설정한다.
        /// </summary>
        /// <param name="datetime">문자형식의 일자 String</param>
        /// <param name="format">datetime에 대한 date format</param>
        /// <returns>this</returns>
        public SyrupPayTokenBuilder IsNotValidBefore(String datetime, String format)
        {
            DateTime to = DateTime.ParseExact(datetime, format, CultureInfo.CurrentCulture);
            DateTime from = new DateTime(1970, 1, 1);
            this.nbf = (long)(to - from).TotalSeconds;
            return this;
        }

        /// <summary>
        ///  JWT 의 nbf 에 해당하는 값으로 토큰이 유효한 시작 시간을 설정한다.
        /// </summary>
        /// <param name="datetime">DateTime 형식의 일자</param>
        /// <returns>this</returns>
        public SyrupPayTokenBuilder IsNotValidBefore(DateTime datetime)
        {
            DateTime to = datetime;
            DateTime from = new DateTime(1970, 1, 1);
            this.nbf = (long)(to - from).TotalSeconds;
            return this;
        }

        /// <summary>
        /// 만료 시간을 생성 시점 이후로 분단위로 입력하여 설정한다. 입력하지 않는 경우 Default 10분으로 설정된다.
        /// </summary>
        /// <param name="expiredMinutes">분단위 만료시간 interval</param>
        /// <returns>this</returns>
        public SyrupPayTokenBuilder ExpiredMinutes(int expiredMinutes)
        {
            this.expiredMinutes = expiredMinutes;
            return this;
        }

        /// <summary>
        /// 시럽페이 로그인을 위한 설정 객체를 확인하여 반환한다.
        /// </summary>
        /// <returns><see cref="MerchantUserConfigurer{H}"/></returns>
        public MerchantUserConfigurer<SyrupPayTokenBuilder> Login()
        {
            return GetOrApply(new MerchantUserConfigurer<SyrupPayTokenBuilder>());
        }

        /// <summary>
        /// 시럽페이 회원가입을 위한 설정 객체를 확인하여 반환한다.
        /// </summary>
        /// <returns><see cref="MerchantUserConfigurer{H}"/></returns>
        public MerchantUserConfigurer<SyrupPayTokenBuilder> SignUp()
        {
            return GetOrApply(new MerchantUserConfigurer<SyrupPayTokenBuilder>());
        }

        /// <summary>
        /// 시럽페이 결제를 위한 설정 객체를 확인하여 반환한다.
        /// </summary>
        /// <returns><see cref="PayConfigurer{H}"/></returns>
        public PayConfigurer<SyrupPayTokenBuilder> Pay()
        {
            return GetOrApply(new PayConfigurer<SyrupPayTokenBuilder>());
        }

        /// <summary>
        /// 시럽페이 체크아웃 기느을 위한 설정 객체를 확인하여 반환한다.
        /// </summary>
        /// <returns><see cref="OrderConfigurer{H}"/></returns>
        public OrderConfigurer<SyrupPayTokenBuilder> Checkout()
        {
            return GetOrApply(new OrderConfigurer<SyrupPayTokenBuilder>());
        }

        /// <summary>
        /// 시럽페이 사용자 맵핑을 위한 설정 객체를 확인하여 반환한다.
        /// </summary>
        /// <returns><see cref="MapToSyrupPayUserConfigurer{H}"/></returns>
        public MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder> MapToSyrupPayUser()
        {
            return GetOrApply(new MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder>());
        }

        /// <summary>
        /// SKT 사용자인 시럽페이 사용자 맵핑을 위한 설정 객체를 확인하여 반환한다.
        /// </summary>
        /// <returns><see cref="MapToSktUserConfigurer{H}"/></returns>
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

        /// <summary>
        /// JWT(JWS-RFC7515) 토큰을 생성하여 반환한다.
        /// </summary>
        /// <param name="secret">Signing 할 Secret</param>
        /// <returns>JWT</returns>
        /// <exception cref="JsonOperationException">json serialize 오류</exception>
        /// <exception cref="IllegalArgumentException">claim 생성중 오류 발생</exception>
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

        /// <summary>
        /// JWT(JWS-RFC7515) 토큰을 생성하여 반환한다.
        /// </summary>
        /// <param name="secret">Signing 할 Secret</param>
        /// <returns>JWT</returns>
        /// <exception cref="JsonOperationException">json serialize 오류</exception>
        /// <exception cref="IllegalArgumentException">claim 생성중 오류 발생</exception>
        public String GenerateTokenBy(byte[] secret)
        {
            return GenerateTokenBy(StringUtils.ByteToString(secret));
        }

        protected String ToJson()
        {
            JObject o = (JObject)JToken.FromObject(Build());

            try
            {
                foreach (Type type in GetClasses())
                {
                    IClaimConfigurer<Jwt, SyrupPayTokenBuilder> configurer = GetConfigurer<IClaimConfigurer<Jwt, SyrupPayTokenBuilder>>(type);
                    configurer.ValidRequired();
                    o.Add(configurer.ClaimName(), (JObject)JToken.FromObject(configurer));
                }
            }
            catch (Exception e)
            {
                throw new IllegalArgumentException(e.Message);
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

                //Debug.WriteLine(traceWriter.ToString());
                return json;
            }
            catch (JsonSerializationException e)
            {
                throw new JsonOperationException(e.Message);
            }
        }
    }
}

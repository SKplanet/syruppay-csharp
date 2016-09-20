using Newtonsoft.Json;
using SyrupPayToken.exception;
using System;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class MerchantUserConfigurer<H> : AbstractTokenConfigurer<MerchantUserConfigurer<H>, H> where H : ITokenBuilder<H>
    {
        private string mctUserId;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string extraUserId;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string implicitSSOSeed;
        [JsonProperty("SSOCredential", NullValueHandling = NullValueHandling.Ignore)]
        private String ssoCredential;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private String deviceIdentifier;

        public MerchantUserConfigurer<H> WithMerchantUserId(String merchantUserId)
        {
            this.mctUserId = merchantUserId;
            return this;
        }

        public MerchantUserConfigurer<H> WithExtraMerchantUserId(String extraMerchantUserId)
        {
            this.extraUserId = extraMerchantUserId;
            return this;
        }

        [Obsolete]
        public MerchantUserConfigurer<H> WithImplicitSSOSeed(String implicitSSOSeed)
        {
            this.implicitSSOSeed = implicitSSOSeed;
            return this;
        }

        public MerchantUserConfigurer<H> WithSsoCredential(String ssoCredential)
        {
            this.ssoCredential = ssoCredential;
            return this;
        }

        public MerchantUserConfigurer<H> WithDeviceIdentifier(String deviceIdentifier)
        {
            this.deviceIdentifier = deviceIdentifier;
            return this;
        }

        public override string ClaimName()
        {
            return "loginInfo";
        }


        public override void ValidRequired()
        {
            if (String.IsNullOrEmpty(mctUserId))
            {
                throw new IllegalArgumentException("when you try to login or sign up, merchant user id couldn't be null. you should set merchant user id  by SyrupPayTokenHandler.login().withMerchantUserId(String) or SyrupPayTokenHandler.signup().withMerchantUserId(String)");
            }
        }
    }
}

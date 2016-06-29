using Newtonsoft.Json;
using SyrupPayToken.Utils;
using static SyrupPayToken.Claims.PayConfigurer<SyrupPayToken.SyrupPayTokenBuilder>;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class SubscriptionConfigurer<H> : AbstractTokenConfigurer<SubscriptionConfigurer<H>, H> where H : ITokenBuilder<H>
    {
        [JsonProperty]
        private string autoPaymentId;
        [JsonProperty]
        private RegistrationRestrictions registrationRestrictions = new RegistrationRestrictions();

        public SubscriptionConfigurer<H> WithAutoPaymentId(string autoPaymentId)
        {
            this.autoPaymentId = autoPaymentId;
            return this;
        }

        public SubscriptionConfigurer<H> WithMatchedUser(MatchedUser m)
        {
            registrationRestrictions.MatchedUser = EnumString<MatchedUser>.GetValue(m);
            return this;
        }

        public string GetAutoPaymentId()
        {
            return autoPaymentId;
        }

        public string GetMatchedUser()
        {
            return registrationRestrictions.MatchedUser;
        }

        public string AutoPaymentId
        {
            get { return autoPaymentId; }
        }

        public string MatchedUserOfRestrictions
        {
            get { return registrationRestrictions.MatchedUser; }
        }

        public override string ClaimName()
        {
            return "subscription";
        }

        public override void ValidRequired()
        {
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class RegistrationRestrictions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string matchedUser;

        public string MatchedUser
        {
            get { return matchedUser; }
            set { matchedUser = value; }
        }
    }
}

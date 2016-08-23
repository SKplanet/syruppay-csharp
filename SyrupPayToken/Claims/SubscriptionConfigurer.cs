using Newtonsoft.Json;
using SyrupPayToken.Utils;
using static SyrupPayToken.Claims.PayConfigurer<SyrupPayToken.SyrupPayTokenBuilder>;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class SubscriptionConfigurer<H> : AbstractTokenConfigurer<SubscriptionConfigurer<H>, H> where H : ITokenBuilder<H>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string mctSubscriptRequestId;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string autoPaymentId;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private Plan plan;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private RegistrationRestrictions registrationRestrictions;

        public SubscriptionConfigurer<H> WithMerchantSubscriptionId(string mctSubscriptRequestId)
        {
            this.mctSubscriptRequestId = mctSubscriptRequestId;
            return this;
        }

        public SubscriptionConfigurer<H> WithAutoPaymentId(string autoPaymentId)
        {
            this.autoPaymentId = autoPaymentId;
            return this;
        }

        private RegistrationRestrictions GetRegistrationRestrictionsOrCreate()
        {
            if (registrationRestrictions == null)
                registrationRestrictions = new RegistrationRestrictions();

            return registrationRestrictions;
        }

        public SubscriptionConfigurer<H> WithMatchedUser(MatchedUser m)
        {
            GetRegistrationRestrictionsOrCreate().MatchedUser = EnumString<MatchedUser>.GetValue(m);
            return this;
        }

        private Plan GetPlanOrCreate()
        {
            if (plan == null)
                plan = new Plan();

            return plan;
        }

        public SubscriptionConfigurer<H> WithInterval(SubscriptionInterval i)
        {
            GetPlanOrCreate().Interval = EnumString<SubscriptionInterval>.GetValue(i);
            return this;
        }

        public SubscriptionConfigurer<H> WithServiceName(string name)
        {
            GetPlanOrCreate().Name = name;
            return this;
        }

        public string GetAutoPaymentId()
        {
            return autoPaymentId;
        }

        public string GetMatchedUser()
        {
            return GetRegistrationRestrictionsOrCreate().MatchedUser;
        }

        public string AutoPaymentId
        {
            get { return autoPaymentId; }
        }

        public string MatchedUserOfRestrictions
        {
            get { return GetRegistrationRestrictionsOrCreate().MatchedUser; }
        }

        public string Interval
        {
            get { return GetPlanOrCreate().Interval; }
        }

        public string ServiceName
        {
            get { return GetPlanOrCreate().Name; }
        }

        public override string ClaimName()
        {
            return "subscription";
        }

        public override void ValidRequired()
        {
        }

        public enum SubscriptionInterval
        {
            [Description("ONDEMAND")]
            ONDEMAND,
            [Description("MONTHLY")]
            MONTHLY,
            [Description("WEEKLY")]
            WEEKLY,
            [Description("BIWEEKLY")]
            BIWEEKLY
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Plan
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string interval;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string name;

        public string Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
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

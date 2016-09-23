using Newtonsoft.Json;
using SyrupPayToken.exception;
using SyrupPayToken.Utils;
using System;

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
        private string promotionCode;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private RegistrationRestrictions registrationRestrictions;

        private Plan GetOrNewPlan
        {
            get
            {
                if (plan == null)
                    plan = new Plan();
                return plan;
            }
        }

        private RegistrationRestrictions GetOrNewRegistrationRestrictions
        {
            get
            {
                if (registrationRestrictions == null)
                    registrationRestrictions = new RegistrationRestrictions();

                return registrationRestrictions;
            }
        }

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

        public SubscriptionConfigurer<H> WithPlanInterval(SubscriptionInterval i)
        {
            GetOrNewPlan.Interval = EnumString<SubscriptionInterval>.GetValue(i);
            return this;
        }

        public SubscriptionConfigurer<H> WithPlanName(string name)
        {
            GetOrNewPlan.Name = name;
            return this;
        }

        public SubscriptionConfigurer<H> WithPromotionCode(string promotionCode)
        {
            this.promotionCode = promotionCode;
            return this;
        }

        public SubscriptionConfigurer<H> WithMatchedUser(MatchedUser m)
        {
            GetOrNewRegistrationRestrictions.MatchedUser = EnumString<MatchedUser>.GetValue(m);
            return this;
        }

        public override string ClaimName()
        {
            return "subscription";
        }

        public override void ValidRequired()
        {
            if (promotionCode != null && promotionCode.Length > 32)
            {
                throw new IllegalArgumentException(String.Format("promotionCode should be less than 32 bytes. Yours promotionCode is {0} bytes.", promotionCode.Length));
            }

            if (plan != null)
                plan.ValidRequired();
        }
    }
}

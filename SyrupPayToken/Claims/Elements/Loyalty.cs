using Newtonsoft.Json;
using SyrupPayToken.exception;
using SyrupPayToken.Utils;
using System;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class Loyalty : Element
    {
        private string id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string userActionCode;
        private string name;
        private string subscriberId;
        private int balance;
        private int maxApplicableAmt;
        private int initialAppliedAmt;
        private int orderApplied;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private bool applicableForNotMatchedUser;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string exclusiveGroupId;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string exclusiveGroupName;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private AdditionalDiscount additionalDiscount;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private Error error;

        public Loyalty SetId(string id)
        {
            this.id = id;
            return this;
        }

        public Loyalty SetIdBy(LoyaltyId loyaltyId)
        {
            SetId(EnumString<LoyaltyId>.GetValue(loyaltyId));
            return this;
        }

        public Loyalty SetUserActionCode(string userActionCode)
        {
            this.userActionCode = userActionCode;
            return this;
        }

        public Loyalty SetName(string name)
        {
            this.name = name;
            return this;
        }

        public Loyalty SetSubscriberId(string subscriberId)
        {
            this.subscriberId = subscriberId;
            return this;
        }

        public Loyalty SetBalance(int balance)
        {
            this.balance = balance;
            return this;
        }

        public Loyalty SetMaxApplicableAmt(int maxApplicableAmt)
        {
            this.maxApplicableAmt = maxApplicableAmt;
            return this;
        }

        public Loyalty SetInitialAppliedAmt(int initialAppliedAmt)
        {
            this.initialAppliedAmt = initialAppliedAmt;
            return this;
        }

        public Loyalty SetOrderApplied(int orderApplied)
        {
            this.orderApplied = orderApplied;
            return this;
        }

        public Loyalty SetApplicableForNotMatchedUser(bool applicableForNotMatchedUser)
        {
            this.applicableForNotMatchedUser = applicableForNotMatchedUser;
            return this;
        }

        public Loyalty SetExclusiveGroupId(string exclusiveGroupId)
        {
            this.exclusiveGroupId = exclusiveGroupId;
            return this;
        }

        public Loyalty SetExclusiveGroupName(string exclusiveGroupName)
        {
            this.exclusiveGroupName = exclusiveGroupName;
            return this;
        }

        public Loyalty SetAdditionalDiscount(AdditionalDiscount additionalDiscount)
        {
            this.additionalDiscount = additionalDiscount;
            return this;

        }

        public Loyalty SetError(Error error)
        {
            this.error = error;
            return this;
        }

        public void ValidRequired()
        {
            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(name) || String.IsNullOrEmpty(subscriberId))
            {
                throw new IllegalArgumentException("Loyalty object couldn't be with null fields id : " + id + ", name : " + name + ", subscriberId : " + subscriberId);
            }

            if (!Object.ReferenceEquals(null, additionalDiscount))
            {
                additionalDiscount.ValidRequired();
            }

            if (!Object.ReferenceEquals(null, error))
            {
                error.ValidRequired();
            }

            if (balance <= 0)
            {
                throw new IllegalArgumentException("balance field should be bigger than 0. yours balance is : " + balance);
            }
            if (maxApplicableAmt <= 0)
            {
                throw new IllegalArgumentException("maxApplicableAmt field should be bigger than 0. yours maxApplicableAmt is : " + maxApplicableAmt);
            }
        }
    }
}

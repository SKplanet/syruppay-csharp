using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using System;
using System.Collections.Generic;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class Offer : Element
    {
        private string id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string userActionCode;
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private OfferType type;
        private string name;
        private int amountOff;
        private bool userSelectable;
        private int orderApplied;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private bool applicableForNotMatchedUser;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string exclusiveGroupId;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string exclusiveGroupName;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private List<Accept> accepted;

        private List<Accept> GetOrNewAcceptedList
        {
            get
            {
                if (accepted == null)
                    accepted = new List<Accept>();
                return accepted;
            }
        }

        public Offer SetId(string id)
        {
            this.id = id;
            return this;
        }

        public Offer SetUserActionCode(string userActionCode)
        {
            this.userActionCode = userActionCode;
            return this;
        }

        public Offer SetType(OfferType type)
        {
            this.type = type;
            return this;
        }

        public Offer SetName(string name)
        {
            this.name = name;
            return this;
        }

        public Offer SetAmountOff(int amountOff)
        {
            this.amountOff = amountOff;
            return this;
        }

        public Offer SetUserSelectable(bool userSelectable)
        {
            this.userSelectable = userSelectable;
            return this;
        }

        public Offer SetOrderApplied(int orderApplied)
        {
            this.orderApplied = orderApplied;
            return this;
        }
        public Offer SetApplicableForNotMatchedUser(bool applicableForNotMatchedUser)
        {
            this.applicableForNotMatchedUser = applicableForNotMatchedUser;
            return this;
        }

        public Offer SetExclusiveGroupId(string exclusiveGroupId)
        {
            this.exclusiveGroupId = exclusiveGroupId;
            return this;
        }

        public Offer SetExclusiveGroupName(string exclusiveGroupName)
        {
            this.exclusiveGroupName = exclusiveGroupName;
            return this;
        }

        public Offer SetAccepted(List<Accept> accepted)
        {
            GetOrNewAcceptedList.AddRange(accepted);
            return this;
        }

        public Offer SetAccepted(params Accept[] accepted)
        {
            SetAccepted(new List<Accept>(accepted));
            return this;
        }

        public void ValidRequired()
        {
            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(name))
            {
                throw new IllegalArgumentException("Offer object couldn't be with null fields id : " + id + ", name : " + name);
            }
            if (amountOff <= 0)
            {
                throw new IllegalArgumentException("amountOff field should be bigger than 0. yours amountOff is : " + amountOff);
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using System;
using System.Collections.Generic;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class Accept : Element
    {
        [JsonConverter(typeof(StringEnumConverter))]
        private AcceptType type = AcceptType.UNDEFINED;
        private List<Dictionary<string, object>> conditions = new List<Dictionary<string, object>>();

        public Accept SetType(AcceptType type)
        {
            this.type = type;
            return this;
        }

        private List<Dictionary<String, Object>> GetConditions()
        {
            return conditions;
        }

        public Accept AddCondition(string cardCode, int minPaymentAmt)
        {
            GetConditions().Add(new Dictionary<string, object>() {
                { "cardCode", cardCode },
                { "minPaymentAmt", minPaymentAmt }
            });

            return this;
        }

        public void ValidRequired()
        {
            if (type == AcceptType.UNDEFINED)
            {
                throw new IllegalArgumentException("Accept object couldn't be with null fields.");
            }

            if (Object.ReferenceEquals(null, conditions))
            {
                throw new IllegalArgumentException("Conditions of Accept object couldn't be empty. you should contain with conditions of Accept object.");
            }
        }
    }
}

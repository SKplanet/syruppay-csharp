using Newtonsoft.Json;
using SyrupPayToken.exception;
using System;
using System.Collections.Generic;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public class MonthlyInstallment : Element
    {
        private string cardCode;
        private List<Dictionary<string, Object>> conditions = new List<Dictionary<string, Object>>();

        public MonthlyInstallment SetCardCode(string cardCode)
        {
            this.cardCode = cardCode;
            return this;
        }

        public MonthlyInstallment AddCondition(int min, bool includeMin, int max, bool includeMax, string monthlyInstallmentInfo)
        {
            Dictionary<string, Object> m = new Dictionary<string, Object>();
            m.Add("paymentAmtRange", (includeMin ? "[" : "(") + min + "-" + max + (includeMax ? "]" : ")"));
            m.Add("monthlyInstallmentInfo", monthlyInstallmentInfo);
            this.conditions.Add(m);
            return this;
        }

        public MonthlyInstallment AddCondition(int min, bool includeMin, string monthlyInstallmentInfo)
        {
            Dictionary<string, Object> m = new Dictionary<string, Object>();
            m.Add("paymentAmtRange", (includeMin ? "[" : "(") + min + "-]");
            m.Add("monthlyInstallmentInfo", monthlyInstallmentInfo);
            this.conditions.Add(m);
            return this;
        }

        public void ValidRequired()
        {
            if (String.IsNullOrEmpty(cardCode))
            {
                throw new IllegalArgumentException("MonthlyInstallment object couldn't be with null fields cardCode is null");
            }

            if (Object.ReferenceEquals(null, conditions) || conditions.Count == 0)
            {
                throw new IllegalArgumentException("Conditions of MonthlyInstallment object couldn't be empty. you should contain with conditions of MonthlyInstallment object by addCondition method.");
            }
        }
    }
}

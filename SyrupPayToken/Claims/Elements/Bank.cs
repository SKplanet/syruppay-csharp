using System;
using Newtonsoft.Json;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class Bank : Element
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string bankCode;

        public Bank SetBankCode(params string[] bankCodes)
        {
            if (bankCodes != null)
            {
                foreach (string bankCode in bankCodes)
                {
                    if (this.bankCode == null)
                        this.bankCode = bankCode;
                    else
                        this.bankCode += ":" + bankCode;
                }
            }

            return this;
        }

        public void ValidRequired()
        {
        }
    }
}

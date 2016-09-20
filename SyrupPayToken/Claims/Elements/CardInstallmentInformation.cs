using System;
using Newtonsoft.Json;
using SyrupPayToken.exception;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class CardInstallmentInformation : Element
    {
        private string cardCode;
        private string monthlyInstallmentInfo;

        public CardInstallmentInformation(string cardCode, string monthlyInstallmentInfo)
        {
            this.cardCode = cardCode;
            this.monthlyInstallmentInfo = monthlyInstallmentInfo;
        }

        public void ValidRequired()
        {
            if (String.IsNullOrEmpty(cardCode))
            {
                throw new IllegalArgumentException("cardCode field should not be null.");
            }

            if (String.IsNullOrEmpty(monthlyInstallmentInfo))
            {
                throw new IllegalArgumentException("monthlyInstallmentInfo field should not be null.");
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public class PayableCard
    {
        private string cardNo;
        private string expireDate;
        private string cardIssuer;
        private string cardIssuerName;
        private string cardName;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string cardNameInEnglish;
        private string cardAcquirer;
        [JsonConverter(typeof(StringEnumConverter))]
        private CardType cardType;

        public PayableCard SetCardNo(string cardNo)
        {
            if (cardNo == null || cardNo.Length == 0)
            {
                throw new IllegalArgumentException("cardNo shouldn't be null and not empty.");
            }
            this.cardNo = cardNo;
            return this;
        }

        public PayableCard SetExpireDate(string expireDate)
        {
            if (expireDate == null && expireDate.Length == 0)
            {
                throw new IllegalArgumentException("expireDate shouldn't be null and not empty.");
            }
            this.expireDate = expireDate;
            return this;
        }

        public PayableCard SetCardIssuer(string cardIssuer)
        {
            if (cardIssuer == null || cardIssuer.Length == 0)
            {
                throw new IllegalArgumentException("cardIssuer shouldn't be null and not empty.");
            }
            this.cardIssuer = cardIssuer;
            return this;
        }

        public PayableCard SetCardIssuerName(string cardIssuerName)
        {
            if (cardIssuerName == null || cardIssuerName.Length == 0)
            {
                throw new IllegalArgumentException("cardIssuerName shouldn't be null and not empty.");
            }
            this.cardIssuerName = cardIssuerName;
            return this;
        }

        public PayableCard SetCardName(string cardName)
        {
            if (cardName == null || cardName.Length == 0)
            {
                throw new IllegalArgumentException("cardNo shouldn't be null and not empty.");
            }
            this.cardName = cardName;
            return this;
        }

        public PayableCard SetCardAcquirer(string cardAcquirer)
        {
            if (cardAcquirer == null || cardAcquirer.Length == 0)
            {
                throw new IllegalArgumentException("cardAcquirer shouldn't be null and not empty.");
            }
            this.cardAcquirer = cardAcquirer;
            return this;
        }

        public PayableCard SetCardType(CardType cardType)
        {
            if (cardType == null)
            {
                throw new IllegalArgumentException("cardType shouldn't be null.");
            }
            this.cardType = cardType;
            return this;
        }

        public string getCardNameInEnglish()
        {
            return cardNameInEnglish;
        }

        public PayableCard SetCardNameInEnglish(string cardNameInEnglish)
        {
            this.cardNameInEnglish = cardNameInEnglish;
            return this;
        }
    }
}

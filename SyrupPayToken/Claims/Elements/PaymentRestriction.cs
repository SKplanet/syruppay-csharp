using Newtonsoft.Json;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class PaymentRestriction
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string cardIssuerRegion = "ALLOWED:KOR";
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string matchedUser;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string paymentType;

        public string CardIssuerRegion
        {
            get { return cardIssuerRegion; }
            set { cardIssuerRegion = value; }
        }

        public string MatchedUser
        {
            get { return matchedUser; }
            set { matchedUser = value; }
        }

        public string PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }
    }
}

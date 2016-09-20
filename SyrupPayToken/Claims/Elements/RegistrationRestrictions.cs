using Newtonsoft.Json;

namespace SyrupPayToken.Claims
{
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

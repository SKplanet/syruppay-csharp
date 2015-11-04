using Newtonsoft.Json;
using System;

namespace SyrupPayToken
{
    [JsonObject(MemberSerialization.Fields)]
    public class Jwt
    {
        private string aud = "https://pay.syrup.co.kr";
        private string typ = "jose";
        private string iss;
        private long exp;
        private long iat;
        private string jti = Guid.NewGuid().ToString();
        private long nbf;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string sub;

        public string Sub
        {
            set { sub = value; }
        }

        public string Iss
        {
            set { iss = value; }
        }

        public long Iat
        {
            set { iat = value; }
            get { return iat; }
        }

        public long Exp
        {
            set { exp = value; }
            get { return exp; }
        }

        public long Nbf
        {
            set { nbf = value; }
        }
    }
}

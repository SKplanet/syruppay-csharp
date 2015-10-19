using System;
using System.Collections.Specialized;
using Newtonsoft.Json;
using SyrupPayJose.Utils;
using SyrupPayJose.Jwa;

namespace SyrupPayJose
{
    public class JoseHeaderSpec
    {
        public const string ALG = "alg";
        public const string ENC = "enc";
        public const string KID = "kid";
        public const string TYP = "typ";
    }

    public class JoseHeader
    {
        private OrderedDictionary dictionary;
        private Boolean isPrint = false;

        public JoseHeader(JsonWebAlgorithm alg = 0, JsonWebAlgorithm enc = 0, string kid = null)
        {
            if (dictionary == null)
                dictionary = new OrderedDictionary();

            if (alg > 0)
                SetHeader(JoseHeaderSpec.ALG, EnumString.GetValue(alg));

            if (enc > 0)
                SetHeader(JoseHeaderSpec.ENC, EnumString.GetValue(enc));

            if (!String.IsNullOrEmpty(kid))
                SetHeader(JoseHeaderSpec.KID, kid);
        }

        public void SetHeader(string key, string value)
        {
            if (dictionary.Contains(key))
                dictionary.Remove(key);
            dictionary.Add(key, value);
        }

        public string GetHeader(string key)
        {
            if (dictionary.Contains(key))
            {
                return dictionary[key] as string;
            }

            return null;
        }

        public string GetSerialize()
        {
            var json = JsonConvert.SerializeObject(dictionary);

            if (isPrint)
                Console.WriteLine(json);

            var b = StringUtils.StringToByte(json);
            return Base64.base64urlencode(b);
        }

        public void SetSerialize(string src)
        {
            var json = Base64.base64urldecode(src);
            this.dictionary = JsonConvert.DeserializeObject<OrderedDictionary>(StringUtils.ByteToString(json));
        }

        public string Alg
        {
            get { return GetHeader(JoseHeaderSpec.ALG); }
            set { SetHeader(JoseHeaderSpec.ALG, value); }
        }

        public string Enc
        {
            get { return GetHeader(JoseHeaderSpec.ENC); }
            set { SetHeader(JoseHeaderSpec.ENC, value); }
        }

        public string Kid
        {
            get { return GetHeader(JoseHeaderSpec.KID); }
            set { SetHeader(JoseHeaderSpec.KID, value); }
        }

        public Boolean IsPrint
        {
            set { isPrint = value; }
        }
    }
}

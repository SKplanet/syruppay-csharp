using SyrupPayJose;
using SyrupPayToken;
using SyrupPayToken.Claims;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Runtime.InteropServices;
using System.Collections;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using SyrupPayToken.exception;

[assembly: ApplicationActivation(ActivationOption.Server)]
[assembly: ApplicationAccessControl(false)]
namespace SyrupPay
{
    public class JoseSerializer : ServicedComponent
    {
        public string toJwe(string iss, string alg, string enc, string key, string payload)
        {
            JoseHeader header = new JoseHeader();
            header.SetHeader(JoseHeaderSpec.ALG, alg);
            header.SetHeader(JoseHeaderSpec.ENC, enc);
            header.SetHeader(JoseHeaderSpec.KID, iss);

            return new Jose().Configuration(
                JoseBuilders.JsonEncryptionCompactSerializationBuilder()
                    .Header(header)
                    .Payload(payload)
                    .Key(key)
                ).Serialization();
        }

        public string toJws(string iss, string alg, string key, string payload)
        {
            JoseHeader header = new JoseHeader();
            header.SetHeader(JoseHeaderSpec.ALG, alg);
            header.SetHeader(JoseHeaderSpec.KID, iss);

            return new Jose().Configuration(
                JoseBuilders.JsonSignatureCompactSerializationBuilder()
                    .Header(header)
                    .Payload(payload)
                    .Key(key)
                ).Serialization();
        }
    }

    public class JoseDeserializer : ServicedComponent
    {
        public string fromJose(string key, string src)
        {
            return new Jose().Configuration(
                JoseBuilders.CompactDeserializationBuilder()
                    .SerializedSource(src)
                    .Key(key)
                ).Deserialization();
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class MctTAToken : ServicedComponent
    {
        [JsonProperty]
        private string iss;
        [JsonProperty]
        private string aud = "https://pay.syrup.co.kr";
        [JsonProperty]
        private string typ = "application/jose";
        [JsonProperty]
        private long exp;
        [JsonProperty]
        private long iat;
        [JsonProperty(PropertyName = "loginInfo", NullValueHandling = NullValueHandling.Ignore)]
        private LoginInfo loginInfoClaim = new LoginInfo();
        [JsonProperty(PropertyName = "userInfoMapper", NullValueHandling = NullValueHandling.Ignore)]
        private UserInfoMapper userInfoMapperClaim;
        [JsonProperty(PropertyName = "transactionInfo", NullValueHandling = NullValueHandling.Ignore)]
        private TransactionInfo transactionInfoClaim;
        [JsonProperty(PropertyName = "subscription", NullValueHandling = NullValueHandling.Ignore)]
        private Subscription subscriptionClaim;

        public string Iss
        {
            get { return iss; }
            set { iss = value; }
        }

        public long Exp
        {
            get { return exp; }
        }

        public long Iat
        {
            get { return iat; }
            set
            {
                iat = value;
                exp = iat + 600;
            }
        }

        private LoginInfo loginInfo
        {
            get { return loginInfoClaim == null ? loginInfoClaim = new LoginInfo() : loginInfoClaim; }
        }

        private UserInfoMapper userInfoMapper
        {
            get { return userInfoMapperClaim == null ? userInfoMapperClaim = new UserInfoMapper() : userInfoMapperClaim; }
        }

        private TransactionInfo transactionInfo
        {
            get { return transactionInfoClaim == null ? transactionInfoClaim = new TransactionInfo() : transactionInfoClaim; }
        }

        private Subscription subscription
        {
            get { return subscriptionClaim == null ? subscriptionClaim = new Subscription() : subscriptionClaim; }
        }

        public string MctUserId
        {
            get { return loginInfo.mctUserId; }
            set { loginInfo.mctUserId = value; }
        }

        public string ExtraUserId
        {
            get { return loginInfo.extraUserId; }
            set { loginInfo.extraUserId = value; }
        }

        public string ImplicitSSOSeed
        {
            get { return loginInfo.implicitSSOSeed; }
            set { loginInfo.implicitSSOSeed = value; }
        }

        public string SSOCredential
        {
            get { return loginInfo.SSOCredential; }
            set { loginInfo.SSOCredential = value; }
        }

        public string DeviceIdentifier
        {
            get { return loginInfo.deviceIdentifier; }
            set { loginInfo.deviceIdentifier = value; }
        }

        public string MappingType
        {
            get { return userInfoMapper.mappingType; }
            set { userInfoMapper.mappingType = value; }
        }

        public string MappingValue
        {
            get { return userInfoMapper.mappingValue; }
            set { userInfoMapper.mappingValue = value; }
        }

        public string MctTransAuthId
        {
            get { return transactionInfo.mctTransAuthId; }
            set { transactionInfo.mctTransAuthId = value; }
        }

        public string MctDefinedValue
        {
            get { return transactionInfo.mctDefinedValue; }
            set { transactionInfo.mctDefinedValue = value; }
        }

        public string ProductTitle
        {
            get { return transactionInfo.paymentInfo.productTitle; }
            set { transactionInfo.paymentInfo.productTitle = value; }
        }

        public string ProductUrl
        {
            set
            {
                if (transactionInfo.paymentInfo.productUrls == null)
                    transactionInfo.paymentInfo.productUrls = new List<string>();
                transactionInfo.paymentInfo.productUrls.Add(value);
            }
        }

        public List<string> ProductUrls
        {
            get { return transactionInfo.paymentInfo.productUrls; }
        }

        public string Lang
        {
            get { return transactionInfo.paymentInfo.lang; }
            set { transactionInfo.paymentInfo.lang = value; }
        }

        public string CurrencyCode
        {
            get { return transactionInfo.paymentInfo.currencyCode; }
            set { transactionInfo.paymentInfo.currencyCode = value; }
        }

        public long PaymentAmt
        {
            get { return transactionInfo.paymentInfo.paymentAmt; }
            set { transactionInfo.paymentInfo.paymentAmt = value; }
        }

        public string ShippingAddress
        {
            get { return transactionInfo.paymentInfo.shippingAddress; }
            set { transactionInfo.paymentInfo.shippingAddress = value; }
        }

        public string DeliveryPhoneNumber
        {
            get { return transactionInfo.paymentInfo.deliveryPhoneNumber; }
            set { transactionInfo.paymentInfo.deliveryPhoneNumber = value; }
        }

        public string DeliveryName
        {
            get { return transactionInfo.paymentInfo.deliveryName; }
            set { transactionInfo.paymentInfo.deliveryName = value; }
        }

        public void AddCardInfo(string cardCode, string monthlyInstallmentInfo)
        {
            if (transactionInfo.paymentInfo.cardInfoList == null)
                transactionInfo.paymentInfo.cardInfoList = new List<CardInfo>();

            transactionInfo.paymentInfo.cardInfoList.Add(new CardInfo(cardCode, monthlyInstallmentInfo));
        }

        public List<Hashtable> getCardInfo()
        {
            if (transactionInfo.paymentInfo.cardInfoList == null)
                return null;

            List<Hashtable> hts = new List<Hashtable>();
            
            transactionInfo.paymentInfo.cardInfoList.ForEach(c => {
                Hashtable ht = new Hashtable();
                ht.Add("cardCode", c.cardCode);
                ht.Add("monthlyInstallmentInfo", c.monthlyInstallmentInfo);
                hts.Add(ht);
            });

            return hts;
        }

        public bool IsExchangeable
        {
            get { return transactionInfo.paymentInfo.isExchangeable; }
            set { transactionInfo.paymentInfo.isExchangeable = value; }
        }

        public string CardIssuerRegion
        {
            get
            {
                if (transactionInfo.paymentRestrictions == null) return "";
                else return transactionInfo.paymentRestrictions.cardIssuerRegion;
            }
            set
            {
                if (transactionInfo.paymentRestrictions == null)
                    transactionInfo.paymentRestrictions = new PaymentRestrictions();
                transactionInfo.paymentRestrictions.cardIssuerRegion = value;
            }
        }

        public string PaymentInfoMatchedUser
        {
            get
            {
                if (transactionInfo.paymentRestrictions == null) return "";
                else return transactionInfo.paymentRestrictions.matchedUser;
            }
            set
            {
                if (transactionInfo.paymentRestrictions == null)
                    transactionInfo.paymentRestrictions = new PaymentRestrictions();
                transactionInfo.paymentRestrictions.matchedUser = value;
            }
        }

        public string AutoPaymentId
        {
            get
            {
                if (subscription == null) return "";
                else return subscription.autoPaymentId;
            }
            set { subscription.autoPaymentId = value; }
        }

        public string AutoPaymentMatchedUser
        {
            get
            {
                if (subscription == null) return "";
                else if (subscription.registrationRestrictions == null) return "";
                else return subscription.registrationRestrictions.matchedUser;
            }
            set
            {
                if (subscription.registrationRestrictions == null)
                    subscription.registrationRestrictions = new RegistrationRestrictions();

                subscription.registrationRestrictions.matchedUser= value;
            }
        }

        public string ToJson()
        {
            if (iat == 0 || exp == 0)
                throw new IllegalArgumentException("iat, exp is required field.");

            loginInfoClaim.validate();

            if (!Object.ReferenceEquals(null, userInfoMapperClaim))
            {
                userInfoMapper.validate();
            }

            if (!Object.ReferenceEquals(null, transactionInfoClaim))
            {
                transactionInfoClaim.validate();
            }

            return JsonConvert.SerializeObject(this);
        }

        public string Serialzie(string key)
        {
            JoseSerializer serializer = new JoseSerializer();
            return serializer.toJws(iss, "HS256", key, ToJson());
        }

        class LoginInfo
        {
            [JsonProperty]
            public string mctUserId;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string extraUserId;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string implicitSSOSeed;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string SSOCredential;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string deviceIdentifier;

            public void validate()
            {
                if (mctUserId == null)
                    throw new IllegalArgumentException("when you try to login or sign up, mctUserId couldn't be null. you should set mctUserId");
            }
        }

        class UserInfoMapper
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string mappingType;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string mappingValue;

            internal void validate()
            {
                if (String.IsNullOrEmpty(mappingType) ||String.IsNullOrEmpty(mappingValue))
                    throw new IllegalArgumentException(String.Format("mappingType fields to map couldn't be null. type : {0} value : {1}", mappingType, mappingValue));

                if (!String.Equals("CI_MAPPED_KEY", mappingType) && !String.Equals("CI_HASH", mappingType))
                    throw new IllegalArgumentException("mappingType must be one of CI_MAPPED_KEY, CI_HASH");

            }
        }

        class TransactionInfo
        {
            [JsonProperty]
            public string mctTransAuthId;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string mctDefinedValue;
            [JsonProperty]
            public PaymentInfo paymentInfo = new PaymentInfo();
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public PaymentRestrictions paymentRestrictions;

            internal void validate()
            {
                if (String.IsNullOrEmpty(mctTransAuthId) ||
                    String.IsNullOrEmpty(paymentInfo.productTitle) ||
                    String.IsNullOrEmpty(paymentInfo.lang) ||
                    String.IsNullOrEmpty(paymentInfo.currencyCode) ||
                    paymentInfo.paymentAmt <= 0)
                {
                    throw new IllegalArgumentException("some of required fields is null or wrong. " +
                            "you should set mctTransAuthId : " + mctTransAuthId
                            + ",  productTitle : " + paymentInfo.productTitle
                            + ",  lang : " + paymentInfo.lang
                            + ",  currencyCode : " + paymentInfo.currencyCode
                            + ",  paymentAmt : " + paymentInfo.paymentAmt
                    );
                }

                if (mctTransAuthId.Length > 40)
                    throw new IllegalArgumentException("order id of merchant couldn't be longer than 40. but yours is " + mctTransAuthId.Length);

                if (!String.IsNullOrEmpty(mctDefinedValue) && mctDefinedValue.Length > 1024)
                    throw new IllegalArgumentException("merchant define value's length couldn't be bigger than 1024. but yours is " + mctDefinedValue.Length);

            }
        }

        class PaymentInfo
        {
            [JsonProperty]
            public string productTitle;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<string> productUrls;
            [JsonProperty]
            public string lang;
            [JsonProperty]
            public string currencyCode;
            [JsonProperty]
            public long paymentAmt;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string shippingAddress;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string deliveryPhoneNumber;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string deliveryName;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<CardInfo> cardInfoList;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool isExchangeable;

        }

        class CardInfo
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string cardCode;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string monthlyInstallmentInfo;

            public CardInfo(string cardCode, string monthlyInstallmentInfo)
            {
                this.cardCode = cardCode;
                this.monthlyInstallmentInfo = monthlyInstallmentInfo;
            }
        }

        class PaymentRestrictions
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string cardIssuerRegion;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string matchedUser;
        }

        class Subscription
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string autoPaymentId;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public RegistrationRestrictions registrationRestrictions;
        }

        class RegistrationRestrictions
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string matchedUser;
        }
    }
}

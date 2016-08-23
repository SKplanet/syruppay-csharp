using SyrupPayJose;
using SyrupPayToken;
using SyrupPayToken.Claims;
using System;
using System.EnterpriseServices;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

[assembly: ApplicationActivation(ActivationOption.Server)]
[assembly: ApplicationAccessControl(false)]
namespace SyrupPay
{
    public interface IJoseSeriazlier
    {
        string toJwe(string iss, string alg, string enc, string key, string payload);
        string toJws(string iss, string alg, string key, string payload);
    }

    public interface IJoseDeserizlier
    {
        string fromJose(string key, string src);
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class JoseSerializer : ServicedComponent, IJoseSeriazlier
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

    [ClassInterface(ClassInterfaceType.None)]
    public class JoseDeserializer : ServicedComponent, IJoseDeserizlier
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

    public interface IMctTAToken
    {
        string Iss { set; }
        string MctUserId { set; }
        string ExtraUserId { set; }
        string ImplicitSSOSeed { set; }
        string SSOCredential { set; }
        string DeviceIdentifier { set; }
        string MappingType { set; }
        string MappingValue { set; }
        string MctTransAuthId { set; }
        string MctDefinedValue { set; }
        string ProductTitle { set; }
        string ProductUrl { set; }
        string Lang { set; }
        string CurrencyCode { set; }
        int PaymentAmt { set; }
        string ShippingAddress { set; }
        string DeliveryPhoneNumber { set; }
        string DeliveryName { set; }
        void AddCardInfo(string cardCode, string monthlyInstallmentInfo);
        bool IsExchangeable { set; }
        string CardIssuerRegion { set; }
        string PaymentInfoMatchedUser { set; }
        string PaymentType { set; }
        string AutoPaymentId { set; }
        string AutoPaymentMatchedUser { set; }
        string SubscriptionInterval { set; }
        string SubscriptionServiceName { set; }
        string ToJson();
        string Serialzie(string key);
    }

    [ClassInterface(ClassInterfaceType.None)]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class MctTAToken : ServicedComponent, IMctTAToken
    {
        private string json;
        private SyrupPayTokenBuilder builder;
        private MerchantUserConfigurer<SyrupPayTokenBuilder> merchantUserConfigure;
        private PayConfigurer<SyrupPayTokenBuilder> payConfigure;
        private OrderConfigurer<SyrupPayTokenBuilder> orderConfigure;
        private MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder> mapToSyrupPayUserConfigure;
        private SubscriptionConfigurer<SyrupPayTokenBuilder> subscriptionConfigure;

        private SyrupPayTokenBuilder Builder
        {
            get { return this.builder == null ? this.builder = new SyrupPayTokenBuilder() : this.builder; }
        }

        private MerchantUserConfigurer<SyrupPayTokenBuilder> MerchantUserConfigure
        {
            get { return this.merchantUserConfigure == null ? this.merchantUserConfigure = builder.Login() : this.merchantUserConfigure; }
        }

        private PayConfigurer<SyrupPayTokenBuilder> PayConfigure
        {
            get { return this.payConfigure == null ? this.payConfigure = builder.Pay() : this.payConfigure; }
        }

        private OrderConfigurer<SyrupPayTokenBuilder> OrderConfigure
        {
            get { return this.orderConfigure == null ? this.orderConfigure = builder.Checkout() : this.orderConfigure; }
        }

        private MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder> MapToSyrupPayUserConfigure
        {
            get { return this.mapToSyrupPayUserConfigure == null ? this.mapToSyrupPayUserConfigure = builder.MapToSyrupPayUser() : this.mapToSyrupPayUserConfigure; }
        }

        private SubscriptionConfigurer<SyrupPayTokenBuilder> SubscriptionConfigure
        {
            get { return this.subscriptionConfigure == null ? this.subscriptionConfigure = builder.Subscription() : this.subscriptionConfigure; }
        }

        public string Iss
        {
            set { Builder.Of(value); }
        }

        public string MctUserId
        {
            set { MerchantUserConfigure.WithMerchantUserId(value); }
        }

        public string ExtraUserId
        {
            set { MerchantUserConfigure.WithExtraMerchantUserId(value); }
        }

        [Obsolete]
        public string ImplicitSSOSeed
        {
            set { MerchantUserConfigure.WithImplicitSSOSeed(value); }
        }

        public string SSOCredential
        {
            set { MerchantUserConfigure.WithSsoCredential(value); }
        }

        public string DeviceIdentifier
        {
            set { MerchantUserConfigure.WithDeviceIdentifier(value); }
        }

        public string MappingType
        {
            set
            {
                MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder>.MappingType mappingType = 
                    (MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder>.MappingType) Enum.Parse(typeof(MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder>.MappingType), value.ToUpper());
                MapToSyrupPayUserConfigure.WithType(mappingType);
            }
        }

        public string MappingValue
        {
            set { MapToSyrupPayUserConfigure.WithValue(value); }
        }

        public string MctTransAuthId
        {
            set { PayConfigure.WithOrderIdOfMerchant(value); }
        }

        public string MctDefinedValue
        {
            set { PayConfigure.WithMerchantDefinedValue(value); }
        }

        public string ProductTitle
        {
            set { PayConfigure.WithProductTitle(value); }
        }

        public string ProductUrl
        {
            set { PayConfigure.WithProductUrls(value); }
        }

        public string Lang
        {
            set
            {
                PayConfigurer<SyrupPayTokenBuilder>.Language lang = 
                    (PayConfigurer<SyrupPayTokenBuilder>.Language) Enum.Parse(typeof(PayConfigurer<SyrupPayTokenBuilder>.Language), value.ToUpper());

                PayConfigure.WithLanguageForDisplay(lang);
            }
        }

        public string CurrencyCode
        {
            set
            {
                PayConfigurer<SyrupPayTokenBuilder>.Currency currency =
                    (PayConfigurer<SyrupPayTokenBuilder>.Currency)Enum.Parse(typeof(PayConfigurer<SyrupPayTokenBuilder>.Currency), value.ToUpper());

                PayConfigure.WithCurrency(currency);
            }
        }

        public int PaymentAmt
        {
            set { PayConfigure.WithAmount(value); }
        }

        public string ShippingAddress
        {
            set { PayConfigure.WithShippingAddress(value); }
        }

        public string DeliveryPhoneNumber
        {
            set { PayConfigure.WithDeliveryPhoneNumber(value); }
        }

        public string DeliveryName
        {
            set { PayConfigure.WithDeliveryName(value); }
        }

        public void AddCardInfo(string cardCode, string monthlyInstallmentInfo)
        {
            PayConfigure.WithInstallmentPerCardInformation(new PayConfigurer<SyrupPayTokenBuilder>.CardInstallmentInformation(cardCode, monthlyInstallmentInfo));
        }

        public bool IsExchangeable
        {
            set { PayConfigure.WithBeAbleToExchangeToCash(value); }
        }

        public string CardIssuerRegion
        {
            set { PayConfigure.WithPayableRuleWithCard(value.ToUpper()); }
        }

        public string PaymentInfoMatchedUser
        {
            set
            {
                PayConfigurer<SyrupPayTokenBuilder>.MatchedUser matchedUser =
                    (PayConfigurer<SyrupPayTokenBuilder>.MatchedUser)Enum.Parse(typeof(PayConfigurer<SyrupPayTokenBuilder>.MatchedUser), value.ToUpper());

                PayConfigure.WithMatchedUser(value);
            }
        }

        public string PaymentType
        {
            set { PayConfigure.WithRestrictionPaymentType(value); }
        }

        public string AutoPaymentId
        {
            set { SubscriptionConfigure.WithAutoPaymentId(value); }
        }

        public string SubscriptionInterval
        {
            set
            {
                SubscriptionConfigurer<SyrupPayTokenBuilder>.SubscriptionInterval interval =
                  (SubscriptionConfigurer<SyrupPayTokenBuilder>.SubscriptionInterval)Enum.Parse(typeof(SubscriptionConfigurer<SyrupPayTokenBuilder>.SubscriptionInterval), value.ToUpper());
                SubscriptionConfigure.WithInterval(interval);
            }
        }

        public string SubscriptionServiceName
        {
            set
            {
                SubscriptionConfigure.WithServiceName(value);
            }
        }

        public string AutoPaymentMatchedUser
        {
            set
            {
                PayConfigurer<SyrupPayTokenBuilder>.MatchedUser matchedUser =
                    (PayConfigurer<SyrupPayTokenBuilder>.MatchedUser)Enum.Parse(typeof(PayConfigurer<SyrupPayTokenBuilder>.MatchedUser), value.ToUpper());
                SubscriptionConfigure.WithMatchedUser(matchedUser);
            }
        }

        public string MctSubscriptRequestId
        {
            set { SubscriptionConfigure.WithMerchantSubscriptionId(value); }
        }

        public string ToJson()
        {
            this.json = builder.ToJson();
            return json;
        }

        public string Serialzie(string key)
        {
            if (json != null)
            {
                string jwt = builder.GenerateTokenBy(key, json);
                json = null;

                return jwt;
            }
            else
            {
                return builder.GenerateTokenBy(key);
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using SyrupPayToken.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class PayConfigurer<H> : AbstractTokenConfigurer<PayConfigurer<H>, H> where H : ITokenBuilder<H>
    {
        private static HashSet<string> ISO_LANGUAGES;
        private static HashSet<string> ISO_COUNTRIES;

        [JsonProperty]
        private string mctTransAuthId;
        [JsonProperty]
        private string mctDefinedValue;
        [JsonProperty]
        private PaymentInformationBySeller paymentInfo = new PaymentInformationBySeller();
        [JsonProperty]
        private PaymentRestriction paymentRestrictions = new PaymentRestriction();

        public PayConfigurer()
        {
            if (Object.ReferenceEquals(null, ISO_LANGUAGES) || ISO_LANGUAGES.Count == 0)
                LoadLanguageByIso639();
            if (Object.ReferenceEquals(null, ISO_COUNTRIES) || ISO_COUNTRIES.Count == 0)
                LoadCountriesByIso3166();
        }

        private void LoadLanguageByIso639()
        {
            ISO_LANGUAGES = new HashSet<string>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (var culture in cultures)
            {
                ISO_LANGUAGES.Add(culture.TwoLetterISOLanguageName);
            }
        }

        public static void LoadCountriesByIso3166()
        {
            List<RegionInfo> countries = new List<RegionInfo>();
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo country = new RegionInfo(culture.LCID);
                if (countries.Where(p => p.Name == country.Name).Count() == 0)
                    countries.Add(country);
            }

            ISO_COUNTRIES = new HashSet<String>();
            foreach (RegionInfo regionInfo in countries)
            {
                ISO_COUNTRIES.Add(regionInfo.TwoLetterISORegionName);
            }
        }

        public static bool IsValidCountryAlpha2Code(string code)
        {
            return ISO_COUNTRIES.Contains(code.Contains(":") ? code.Substring(code.IndexOf(":") + 1).ToUpper() : code.ToUpper());
        }

        public static bool IsValidLanuageCode(string code)
        {
            return ISO_LANGUAGES.Contains(code);
        }

        public string GetMctTransAuthId()
        {
            return mctTransAuthId;
        }

        public string GetMerchanttDefinedValue()
        {
            return mctDefinedValue;
        }


        public PaymentInformationBySeller GetPaymentInfo()
        {
            return paymentInfo;
        }

        public PaymentRestriction GetPaymentRestrictions()
        {
            return paymentRestrictions;
        }

        public PayConfigurer<H> WithOrderIdOfMerchant(string orderId)
        {
            mctTransAuthId = orderId;
            return this;
        }

        public PayConfigurer<H> WithMerchantDefinedValue(string merchantDefinedValue)
        {
            this.mctDefinedValue = merchantDefinedValue;
            return this;
        }

        public PayConfigurer<H> WithProductTitle(string productTitle)
        {
            paymentInfo.ProductTitle = productTitle;
            return this;
        }

        public PayConfigurer<H> WithProductUrls(List<string> productUrls)
        {
            foreach (string productDetail in productUrls)
            {
                if (!(productDetail.StartsWith("http") || productDetail.StartsWith("https")))
                {
                    throw new IllegalArgumentException("product details should be contained http or https urls. check your input!");
                }
            }
            paymentInfo.AddProductUrls = productUrls;
            return this;
        }

        public PayConfigurer<H> WithProductUrls(params String[] url)
        {
            return WithProductUrls(new List<String>(url));
        }

        public PayConfigurer<H> WithLanguageForDisplay(Language l)
        {
            paymentInfo.Lang = l.ToString();
            return this;
        }

        public PayConfigurer<H> WithCurrency(Currency c)
        {
            paymentInfo.CurrencyCode = c.ToString();
            return this;
        }

        public PayConfigurer<H> WithShippingAddress(ShippingAddress shippingAddress)
        {
            paymentInfo.ShippingAddress = shippingAddress.MapToStringForFds;
            return this;
        }

        public PayConfigurer<H> WithShippingAddress(string shippingAddress)
        {
            paymentInfo.ShippingAddress = shippingAddress;
            return this;
        }

        public PayConfigurer<H> WithAmount(int paymentAmount)
        {
            if (paymentAmount <= 0)
            {
                throw new IllegalArgumentException("Cannot be smaller than 0. Check yours input value : " + paymentAmount);
            }
            paymentInfo.PaymentAmt = paymentAmount;
            return this;
        }

        public PayConfigurer<H> WithDeliveryPhoneNumber(string deliveryPhoneNumber)
        {
            paymentInfo.DeliveryPhoneNumber = deliveryPhoneNumber;
            return this;
        }

        public PayConfigurer<H> WithDeliveryName(String deliveryName)
        {
            paymentInfo.DeliveryName = deliveryName;
            return this;
        }

        public PayConfigurer<H> WithBeAbleToExchangeToCash(bool exchangeable)
        {
            paymentInfo.Exchangeable = exchangeable;
            return this;
        }

        public PayConfigurer<H> WithInstallmentPerCardInformation(List<CardInstallmentInformation> cards)
        {
            paymentInfo.AddCardInfoList = cards;
            return this;
        }

        public PayConfigurer<H> WithInstallmentPerCardInformation(params CardInstallmentInformation[] card)
        {
            paymentInfo.AddCardInfoList = new List<CardInstallmentInformation>(card);
            return this;
        }

        public PayConfigurer<H> WithPayableRuleWithCard(PayableLocaleRule r)
        {
            paymentRestrictions.CardIssuerRegion = EnumString<PayableLocaleRule>.GetValue(r);
            return this;
        }

        public override string ClaimName()
        {
            return "transactionInfo";
        }

        public override void ValidRequired()
        {
            if (String.IsNullOrEmpty(this.mctTransAuthId) || 
                String.IsNullOrEmpty(this.paymentInfo.ProductTitle) ||
                String.IsNullOrEmpty(this.paymentInfo.Lang) ||
                String.IsNullOrEmpty(this.paymentInfo.CurrencyCode) ||
                this.paymentInfo.PaymentAmt <= 0) {
                    throw new IllegalArgumentException("some of required fields is null or wrong. " +
                            "you should set orderIdOfMerchant : " + mctTransAuthId
                            + ",  productTitle : " + paymentInfo.ProductTitle
                            + ",  languageForDisplay : " + paymentInfo.Lang
                            + ",  currency : " + paymentInfo.CurrencyCode
                            + ",  amount : " + paymentInfo.PaymentAmt
                    );
                }

            if (mctTransAuthId.Length > 40)
                throw new IllegalArgumentException("order id of merchant couldn't be longer than 40. but yours is " + mctTransAuthId.Length);

            if (!String.IsNullOrEmpty(this.mctTransAuthId) && mctTransAuthId.Length > 1024)
                throw new IllegalArgumentException("merchant define value's length couldn't be bigger than 1024. but yours is " + mctDefinedValue.Length);
        }

        public enum Language
        {
            KO, EN
        }

        public enum Currency
        {
            KRW, USD
        }

        public enum PayableLocaleRule
        {
            [Description("ALLOWED:KOR")]
            ONLY_ALLOWED_KOR,
            [Description("NOT_ALLOWED:KOR")]
            ONLY_NOT_ALLOED_KOR,
            [Description("ALLOWED:USA")]
            ONLY_ALLOWED_USA,
            [Description("NOT_ALLOWED:USA")]
            ONLY_NOT_ALLOED_USA
        }

        public enum DeliveryRestriction
        {
            NOT_FAR_AWAY, FAR_AWAY, FAR_FAR_AWAY
        }

        [JsonObject(MemberSerialization.OptIn)]
        public sealed class ShippingAddress
        {
            private string id;
            private string userActionCode;
            private string name;
            private string countryCode;
            private string zipCode;
            private string mainAddress;
            private string detailAddress;
            private string city;
            private string state;
            private string recipientName;
            private string recipientPhoneNumber;
            [JsonConverter(typeof(StringEnumConverter))]
            private DeliveryRestriction deliveryRestriction;
            private int defaultDeliveryCost;
            private int additionalDeliveryCost;
            private int orderApplied;

            public ShippingAddress(string zipCode, string mainAddress, string detailAddress, string city, string state, string countryCode)
            {
                this.zipCode = zipCode;
                this.mainAddress = mainAddress;
                this.detailAddress = detailAddress;
                this.city = city;
                this.state = state;
                this.countryCode = SetCountryCode(countryCode).GetCountryCode();
            }

            public string GetUserActionCode()
            {
                return userActionCode;
            }

            public ShippingAddress SetUserActionCode(string userActionCode)
            {
                this.userActionCode = userActionCode;
                return this;
            }

            public string GetId()
            {
                return id;
            }

            public ShippingAddress SetId(string id)
            {
                this.id = id;
                return this;
            }

            public string GetName()
            {
                return name;
            }

            public ShippingAddress SetName(string name)
            {
                this.name = name;
                return this;
            }

            public string GetCountryCode()
            {
                return countryCode;
            }

            public ShippingAddress SetCountryCode(string countryCode)
            {
                if (!IsValidCountryAlpha2Code(countryCode))
                {
                    throw new IllegalArgumentException("countryCode should meet the specifications of ISO-3166 Alpha2(as KR, US) except prefix like a2. yours : " + countryCode);
                }
                this.countryCode = countryCode.ToLower();
                return this;
            }

            public string GetZipCode()
            {
                return zipCode;
            }

            public ShippingAddress SetZipCode(string zipCode)
            {
                this.zipCode = zipCode;
                return this;
            }

            public string GetMainAddress()
            {
                return mainAddress;
            }

            public ShippingAddress SetMainAddress(string mainAddress)
            {
                this.mainAddress = mainAddress;
                return this;
            }

            public string GetDetailAddress()
            {
                return detailAddress;
            }

            public ShippingAddress SetDetailAddress(string detailAddress)
            {
                this.detailAddress = detailAddress;
                return this;
            }

            public string GetCity()
            {
                return city;
            }

            public ShippingAddress SetCity(string city)
            {
                this.city = city;
                return this;
            }

            public string GetState()
            {
                return state;
            }

            public ShippingAddress SetState(string state)
            {
                this.state = state;
                return this;
            }

            public string GetRecipientName()
            {
                return recipientName;
            }

            public ShippingAddress SetRecipientName(string recipientName)
            {
                this.recipientName = recipientName;
                return this;
            }

            public string GetRecipientPhoneNumber()
            {
                return recipientPhoneNumber;
            }

            public ShippingAddress SetRecipientPhoneNumber(string recipientPhoneNumber)
            {
                Regex regex = new Regex("^\\d+$");
                if (String.IsNullOrEmpty(recipientPhoneNumber) && !regex.Match(recipientPhoneNumber).Success)
                {
                    throw new IllegalArgumentException("phone number should be contained numbers. remove characters as '-'. yours : " + recipientPhoneNumber);
                }
                this.recipientPhoneNumber = recipientPhoneNumber;
                return this;
            }

            public DeliveryRestriction GetDeliveryRestriction()
            {
                return deliveryRestriction;
            }

            public ShippingAddress SetDeliveryRestriction(DeliveryRestriction deliveryRestriction)
            {
                this.deliveryRestriction = deliveryRestriction;
                return this;
            }

            public int GetDefaultDeliveryCost()
            {
                return defaultDeliveryCost;
            }

            public ShippingAddress SetDefaultDeliveryCost(int defaultDeliveryCost)
            {
                this.defaultDeliveryCost = defaultDeliveryCost;
                return this;
            }

            public int GetAdditionalDeliveryCost()
            {
                return additionalDeliveryCost;
            }

            public ShippingAddress SetAdditionalDeliveryCost(int additionalDeliveryCost)
            {
                this.additionalDeliveryCost = additionalDeliveryCost;
                return this;
            }

            public int GetOrderApplied()
            {
                return orderApplied;
            }

            public ShippingAddress SetOrderApplied(int orderApplied)
            {
                this.orderApplied = orderApplied;
                return this;
            }

            public string MapToStringForFds { get { return GetMapToStringForFds(); } }
            public string GetMapToStringForFds()
            {
                return countryCode + "|" + zipCode + "|" + mainAddress + "|" + detailAddress + "|" + city + "|" + state + "|";
            }

            public void ValidRequiredToCheckout()
            {
                if (String.IsNullOrEmpty(id) ||
                    String.IsNullOrEmpty(name) ||
                    String.IsNullOrEmpty(countryCode) ||
                    String.IsNullOrEmpty(zipCode) ||
                    String.IsNullOrEmpty(mainAddress) ||
                    String.IsNullOrEmpty(detailAddress) ||
                    String.IsNullOrEmpty(recipientName) ||
                    String.IsNullOrEmpty(recipientPhoneNumber))
                {
                    throw new IllegalArgumentException("ShippingAddress object to checkout couldn't be with null fields. id : " + id + ", name : " + name + ", countryCode : " + countryCode + ", zipCode : " + zipCode + ", mainAddress : " + mainAddress + ", detailAddress : " + detailAddress + ", recipientName : " + recipientName + ", recipientPhoneNumber : " + recipientPhoneNumber);
                }

                if (!IsValidCountryAlpha2Code(countryCode))
                {
                    throw new IllegalArgumentException("countryCode should meet the specifications of ISO-3166 Alpha2(as KR, US) except prefix like a2. yours : " + countryCode);
                }

                if (defaultDeliveryCost <= 0)
                {
                    throw new IllegalArgumentException("defaultDeliveryCost field should be bigger than 0. yours : " + defaultDeliveryCost);
                }
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public sealed class CardInstallmentInformation
        {
            [JsonProperty]
            private string cardCode;
            [JsonProperty]
            private string monthlyInstallmentInfo;

            public CardInstallmentInformation(string cardCode, string monthlyInstallmentInfo)
            {
                this.cardCode = cardCode;
                this.monthlyInstallmentInfo = monthlyInstallmentInfo;
            }

            public string CardCode { get { return cardCode; } }
            public string GetCardCode()
            {
                return cardCode;
            }

            public string MonthlyInstallmentInfo { get { return monthlyInstallmentInfo; } }
            public string GetMonthlyInstallmentInfo()
            {
                return monthlyInstallmentInfo;
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public sealed class PaymentInformationBySeller
        {
            [JsonProperty]
            private List<CardInstallmentInformation> cardInfoList = new List<CardInstallmentInformation>();
            [JsonProperty]
            private string productTitle;
            [JsonProperty]
            private List<string> productUrls = new List<string>();
            [JsonProperty]
            private string lang = "KO";
            [JsonProperty]
            private string currencyCode = "KRW";
            [JsonProperty]
            private int paymentAmt;
            [JsonProperty]
            private string shippingAddress;
            [JsonProperty]
            private string deliveryPhoneNumber;
            [JsonProperty]
            private string deliveryName;
            [JsonProperty("isExchangeable")]
            private bool isExchangeable;

            public string ProductTitle
            {
                get { return productTitle; }
                set { productTitle = value; }
            }
            public string GetProductTitle()
            {
                return productTitle;
            }

            public List<string> AddProductUrls
            {
                set { productUrls.AddRange(value); }
            }
            public List<string> ProductUrls
            {
                get { return productUrls; }
            }
            public List<string> GetProductUrls()
            {
                return productUrls;
            }

            public string Lang
            {
                get { return lang; }
                set { lang = value; }
            }
            public string GetLang()
            {
                return lang;
            }

            public string CurrencyCode
            {
                get { return currencyCode; }
                set { currencyCode = value; }
            }
            public string GetCurrencyCode()
            {
                return currencyCode;
            }

            public int PaymentAmt
            {
                get { return paymentAmt; }
                set { paymentAmt = value; }
            }
            public int GetPaymentAmt()
            {
                return paymentAmt;
            }

            public string ShippingAddress
            {
                get { return shippingAddress; }
                set { shippingAddress = value;}
            }
            public string GetShippingAddress()
            {
                return shippingAddress;
            }

            public string DeliveryPhoneNumber
            {
                get { return deliveryPhoneNumber; }
                set { deliveryPhoneNumber = value; }
            }
            public string GetDeliveryPhoneNumber()
            {
                return deliveryPhoneNumber;
            }

            public string DeliveryName
            {
                get { return deliveryName; }
                set { deliveryName = value; }
            }
            public string GetDeliveryName()
            {
                return deliveryName;
            }

            public bool Exchangeable
            {
                get { return isExchangeable; }
                set { isExchangeable = value; }
            }
            public bool IsExchangeable()
            {
                return isExchangeable;
            }

            public List<CardInstallmentInformation> AddCardInfoList { set { cardInfoList.AddRange(value); } }
            public List<CardInstallmentInformation> CardInfoList
            {
                get { return cardInfoList; }
            }
            public List<CardInstallmentInformation> GetCardInfoList()
            {
                return cardInfoList;
            }

            [Obsolete]
            public void SetProductDetails(List<string> productDetails)
            {
                this.productUrls = productDetails;
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public sealed class PaymentRestriction
        {
            [JsonProperty]
            private string cardIssuerRegion = "ALLOWED:KOR";

            public string CardIssuerRegion
            {
                get { return cardIssuerRegion; }
                set { cardIssuerRegion = value; }
            }

            public void SetCardIssuerRegion(string cardIssuerRegion)
            {
                this.cardIssuerRegion = cardIssuerRegion;
            }

            public string GetCardIssueRegion()
            {
                return cardIssuerRegion;
            }
        }
    }


}

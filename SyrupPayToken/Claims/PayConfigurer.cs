using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using SyrupPayToken.Utils;
using System;
using System.Collections.Generic;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class PayConfigurer<H> : AbstractTokenConfigurer<PayConfigurer<H>, H> where H : ITokenBuilder<H>
    {
        private string mctTransAuthId;
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private CashReceiptDisplay cashReceiptDisplay;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string mctDefinedValue;
        private PaymentInformationBySeller paymentInfo;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private PaymentRestriction paymentRestrictions;

        public string MctTransAuthId
        {
            get { return mctTransAuthId; }
        }

        public PaymentInformationBySeller PaymentInfo
        {
            get { return paymentInfo; }
        }

        private PaymentInformationBySeller GetOrNewPaymentInfo
        {
            get
            {
                if (paymentInfo == null)
                    paymentInfo = new PaymentInformationBySeller();
                return paymentInfo;
            }
        }

        private PaymentRestriction GetOrNewPaymentRestrictions
        {
            get
            {
                if (paymentRestrictions == null)
                    paymentRestrictions = new PaymentRestriction();
                return paymentRestrictions;
            }
        }

        public PayConfigurer<H> WithOrderIdOfMerchant(string orderId)
        {
            mctTransAuthId = orderId;
            return this;
        }

        public PayConfigurer<H> WithCashReceiptDisplay(CashReceiptDisplay cashReceiptDisplay)
        {
            this.cashReceiptDisplay = cashReceiptDisplay;
            return this;
        }

        public PayConfigurer<H> WithMerchantDefinedValue(string merchantDefinedValue)
        {
            this.mctDefinedValue = merchantDefinedValue;
            return this;
        }

        public PayConfigurer<H> WithProductTitle(string productTitle)
        {
            GetOrNewPaymentInfo.ProductTitle = productTitle;
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
            GetOrNewPaymentInfo.AddProductUrls = productUrls;
            return this;
        }

        public PayConfigurer<H> WithProductUrls(params String[] url)
        {
            return WithProductUrls(new List<String>(url));
        }

        public PayConfigurer<H> WithLanguageForDisplay(Language l)
        {
            GetOrNewPaymentInfo.Lang = l.ToString();
            return this;
        }

        public PayConfigurer<H> WithCurrency(Currency c)
        {
            GetOrNewPaymentInfo.CurrencyCode = c.ToString();
            return this;
        }

        public PayConfigurer<H> WithAmount(int paymentAmount)
        {
            GetOrNewPaymentInfo.PaymentAmt = paymentAmount;
            return this;
        }

        public PayConfigurer<H> WithShippingAddress(ShippingAddress shippingAddress)
        {
            GetOrNewPaymentInfo.ShippingAddress = shippingAddress.MapToStringForFds;
            return this;
        }

        public PayConfigurer<H> WithShippingAddress(string shippingAddress)
        {
            GetOrNewPaymentInfo.ShippingAddress = shippingAddress;
            return this;
        }
       
        public PayConfigurer<H> WithDeliveryPhoneNumber(string deliveryPhoneNumber)
        {
            GetOrNewPaymentInfo.DeliveryPhoneNumber = deliveryPhoneNumber;
            return this;
        }

        public PayConfigurer<H> WithDeliveryName(string deliveryName)
        {
            GetOrNewPaymentInfo.DeliveryName = deliveryName;
            return this;
        }

        public PayConfigurer<H> WithDeliveryType(DeliveryType deliveryType)
        {
            GetOrNewPaymentInfo.DeliveryType = deliveryType;
            return this;
        }

        
        public PayConfigurer<H> WithInstallmentPerCardInformation(List<CardInstallmentInformation> cards)
        {
            GetOrNewPaymentInfo.AddCardInfoList = cards;
            return this;
        }

        public PayConfigurer<H> WithInstallmentPerCardInformation(params CardInstallmentInformation[] card)
        {
            GetOrNewPaymentInfo.AddCardInfoList = new List<CardInstallmentInformation>(card);
            return this;
        }


        public PayConfigurer<H> WithBankInfoList(List<Bank> bankInfoList)
        {
            GetOrNewPaymentInfo.AddBankInfoList = bankInfoList;
            return this;
        }

        public PayConfigurer<H> WithBankInfoList(params Bank[] bankInfoList)
        {
            WithBankInfoList(new List<Bank>(bankInfoList));
            return this;
        }


        public PayConfigurer<H> WithBeAbleToExchangeToCash(bool exchangeable)
        {
            GetOrNewPaymentInfo.Exchangeable = exchangeable;
            return this;
        }

        public PayConfigurer<H> WithPayableRuleWithCard(PayableLocaleRule r)
        {
            WithPayableRuleWithCard(EnumString<PayableLocaleRule>.GetValue(r));
            return this;
        }

        public PayConfigurer<H> WithPayableRuleWithCard(string cardIssuerRegion)
        {
            GetOrNewPaymentRestrictions.CardIssuerRegion = cardIssuerRegion;
            return this;
        }

        public PayConfigurer<H> WithMatchedUser(MatchedUser m)
        {
            WithMatchedUser(EnumString<MatchedUser>.GetValue(m));
            return this;
        }

        public PayConfigurer<H> WithMatchedUser(string matchedUser)
        {
            GetOrNewPaymentRestrictions.MatchedUser = matchedUser;
            return this;
        }

        public PayConfigurer<H> WithRestrictionPaymentType(params PaymentType[] paymentTypes)
        {
            if (paymentTypes != null)
            {
                string paymentType = null;
                foreach (PaymentType p in paymentTypes)
                {
                    if (paymentType != null)
                    {
                        paymentType += ";";
                    }
                    paymentType += Enum.GetName(typeof(PaymentType), p);
                }

                WithRestrictionPaymentType(paymentType);
            }

            return this;
        }

        public PayConfigurer<H> WithRestrictionPaymentType(string paymentType)
        {
            GetOrNewPaymentRestrictions.PaymentType = paymentType;
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
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class PaymentInformationBySeller
    {
        private string productTitle;
        private List<string> productUrls = new List<string>();
        private string lang = "KO";
        private string currencyCode = "KRW";
        private int paymentAmt;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string shippingAddress;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string deliveryPhoneNumber;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string deliveryName;
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private Nullable<DeliveryType> deliveryType = null;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private List<CardInstallmentInformation> cardInfoList;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private List<Bank> bankInfoList;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private bool isExchangeable;

        public string ProductTitle
        {
            get { return productTitle; }
            set { productTitle = value; }
        }

        public List<string> AddProductUrls
        {
            set { productUrls.AddRange(value); }
        }

        public List<string> ProductUrls
        {
            get { return productUrls; }
        }

        public string Lang
        {
            get { return lang; }
            set { lang = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        public int PaymentAmt
        {
            get { return paymentAmt; }
            set { paymentAmt = value; }
        }

        public string ShippingAddress
        {
            get { return shippingAddress; }
            set { shippingAddress = value; }
        }

        public string DeliveryPhoneNumber
        {
            get { return deliveryPhoneNumber; }
            set { deliveryPhoneNumber = value; }
        }

        public string DeliveryName
        {
            get { return deliveryName; }
            set { deliveryName = value; }
        }

        public DeliveryType DeliveryType
        {
            get { return deliveryType.GetValueOrDefault(); }
            set { deliveryType = value; }
        }


        private List<CardInstallmentInformation> GetOrNewCardInfoList
        {
            get
            {
                if (cardInfoList == null)
                    cardInfoList = new List<CardInstallmentInformation>();

                return cardInfoList;
            }
        }

        public List<CardInstallmentInformation> AddCardInfoList { set { GetOrNewCardInfoList.AddRange(value); } }

        public List<CardInstallmentInformation> CardInfoList
        {
            get { return GetOrNewCardInfoList; }
        }

        private List<Bank> GetOrNewBankInfoList
        {
            get
            {
                if (bankInfoList == null)
                    BankInfoList = new List<Bank>();
                return bankInfoList;
            }
        }
        public List<Bank> AddBankInfoList { set { GetOrNewBankInfoList.AddRange(value); } }
        public List<Bank> BankInfoList
        {
            get { return GetOrNewBankInfoList; }
            set { bankInfoList = value; }
        }

        public bool Exchangeable
        {
            get { return isExchangeable; }
            set { isExchangeable = value; }
        }

        [Obsolete]
        public void SetProductDetails(List<string> productDetails)
        {
            this.productUrls = productDetails;
        }
    }
}

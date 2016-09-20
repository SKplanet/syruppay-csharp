using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using System;
using System.Text.RegularExpressions;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class ShippingAddress
    {
        private string id;
        private string name;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string userActionCode;
        private string countryCode;
        private string zipCode;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string city;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string state;
        private string mainAddress;
        private string detailAddress;
        private string recipientName;
        private string recipientPhoneNumber;
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private DeliveryRestriction deliveryRestriction;
        private int defaultDeliveryCost;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private int additionalDeliveryCost;
        private int orderApplied;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string fullAddressFormat;

        public ShippingAddress() { }

        public ShippingAddress(string zipCode, string mainAddress, string detailAddress, string city, string state, string countryCode)
        {
            this.zipCode = zipCode;
            this.mainAddress = mainAddress;
            this.detailAddress = detailAddress;
            this.city = city;
            this.state = state;
            this.countryCode = SetCountryCode(countryCode).countryCode;
        }

        public ShippingAddress SetUserActionCode(string userActionCode)
        {
            this.userActionCode = userActionCode;
            return this;
        }

        public ShippingAddress SetId(string id)
        {
            this.id = id;
            return this;
        }

        public ShippingAddress SetName(string name)
        {
            this.name = name;
            return this;
        }

        public ShippingAddress SetCountryCode(string countryCode)
        {
            if (!ISOCode.IsValidCountryAlpha2Code(countryCode))
            {
                throw new IllegalArgumentException("countryCode should meet the specifications of ISO-3166 Alpha2(as KR, US) except prefix like a2. yours : " + countryCode);
            }
            this.countryCode = countryCode.ToLower();
            return this;
        }

        public ShippingAddress SetZipCode(string zipCode)
        {
            this.zipCode = zipCode;
            return this;
        }

        public ShippingAddress SetMainAddress(string mainAddress)
        {
            this.mainAddress = mainAddress;
            return this;
        }

        public ShippingAddress SetDetailAddress(string detailAddress)
        {
            this.detailAddress = detailAddress;
            return this;
        }

        public ShippingAddress SetCity(string city)
        {
            this.city = city;
            return this;
        }

        public ShippingAddress SetState(string state)
        {
            this.state = state;
            return this;
        }

        public ShippingAddress SetRecipientName(string recipientName)
        {
            this.recipientName = recipientName;
            return this;
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

        public ShippingAddress SetDeliveryRestriction(DeliveryRestriction deliveryRestriction)
        {
            this.deliveryRestriction = deliveryRestriction;
            return this;
        }

        public ShippingAddress SetDefaultDeliveryCost(int defaultDeliveryCost)
        {
            this.defaultDeliveryCost = defaultDeliveryCost;
            return this;
        }

        public ShippingAddress SetAdditionalDeliveryCost(int additionalDeliveryCost)
        {
            this.additionalDeliveryCost = additionalDeliveryCost;
            return this;
        }

        public ShippingAddress SetOrderApplied(int orderApplied)
        {
            this.orderApplied = orderApplied;
            return this;
        }

        public string MapToStringForFds
        {
            set { fullAddressFormat = value; }
            get { return GetMapToStringForFds(); }
        }
        public string GetMapToStringForFds()
        {
            fullAddressFormat = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|", countryCode, zipCode, mainAddress, detailAddress, city, state);
            return fullAddressFormat;
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

            if (!ISOCode.IsValidCountryAlpha2Code(countryCode))
            {
                throw new IllegalArgumentException("countryCode should meet the specifications of ISO-3166 Alpha2(as KR, US) except prefix like a2. yours : " + countryCode);
            }

            if (defaultDeliveryCost <= 0)
            {
                throw new IllegalArgumentException("defaultDeliveryCost field should be bigger than 0. yours : " + defaultDeliveryCost);
            }
        }
    }
}

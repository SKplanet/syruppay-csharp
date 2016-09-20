using Newtonsoft.Json;
using SyrupPayToken.exception;
using System;
using System.Collections.Generic;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class OrderConfigurer<H> : AbstractTokenConfigurer<MerchantUserConfigurer<H>, H> where H : ITokenBuilder<H>
    {
        private int productPrice;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string submallName;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string privacyPolicyRequirements;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private bool mainShippingAddressSettingDisabled;
        private ProductDeliveryInfo productDeliveryInfo;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private List<Offer> offerList;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private List<Loyalty> loyaltyList;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private List<ShippingAddress> shippingAddressList;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private List<MonthlyInstallment> monthlyInstallmentList;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private List<Bank> bankInfoList;

        private List<ShippingAddress> GetOrNewShippingAddressList
        {
            get
            {
                if (shippingAddressList == null)
                    shippingAddressList = new List<ShippingAddress>();
                return shippingAddressList;
            }
        }

        private List<Offer> GetOrNewOfferList
        {
            get
            {
                if (offerList == null)
                    offerList = new List<Offer>();
                return offerList;
            }
        }

        private List<Loyalty> GetOrNewLoyaltyList
        {
            get
            {
                if (loyaltyList == null)
                    loyaltyList = new List<Loyalty>();
                return loyaltyList;
            }
        }

        private List<MonthlyInstallment> GetOrNewMonthlyInstallmentList
        {
            get
            {
                if (monthlyInstallmentList == null)
                    monthlyInstallmentList = new List<MonthlyInstallment>();
                return monthlyInstallmentList;
            }
        }

        private List<Bank> GetOrNewBankInfoList
        {
            get
            {
                if (bankInfoList == null)
                    bankInfoList = new List<Bank>();
                return bankInfoList;
            }
        }

        public OrderConfigurer<H> WithProductPrice(int productPrice)
        {
            this.productPrice = productPrice;
            return this;
        }

        public OrderConfigurer<H> WithSubmallName(string submallName)
        {
            this.submallName = submallName;
            return this;
        }

        public OrderConfigurer<H> WithPrivacyPolicyRequirements(string privacyPolicyRequirements)
        {
            this.privacyPolicyRequirements = privacyPolicyRequirements;
            return this;
        }

        public OrderConfigurer<H> DisableMainShippingAddressSetting()
        {
            this.mainShippingAddressSettingDisabled = true;
            return this;
        }

        public OrderConfigurer<H> EnableMainShippingAddressSetting()
        {
            this.mainShippingAddressSettingDisabled = false;
            return this;
        }

        public OrderConfigurer<H> WithProductDeliveryInfo(ProductDeliveryInfo productDeliveryInfo)
        {
            this.productDeliveryInfo = productDeliveryInfo;
            return this;
        }

        public OrderConfigurer<H> WithOffers(params Offer[] offer)
        {
            return WithOffers(new List<Offer>(offer));
        }

        public OrderConfigurer<H> WithOffers(List<Offer> offers)
        {
            foreach (Offer o in offers)
            {
                o.ValidRequired();
            }
            GetOrNewOfferList.AddRange(offers);
            return this;
        }

        public OrderConfigurer<H> WithLoyalties(params Loyalty[] loyalty)
        {
            return WithLoyalties(new List<Loyalty>(loyalty));
        }

        public OrderConfigurer<H> WithLoyalties(List<Loyalty> loyalties)
        {
            foreach (Loyalty l in loyalties)
            {
                l.ValidRequired();
            }
            GetOrNewLoyaltyList.AddRange(loyalties);
            return this;
        }

        public OrderConfigurer<H> WithShippingAddresses(params ShippingAddress[] shippingAddress)
        {
            return WithShippingAddresses(new List<ShippingAddress>(shippingAddress));
        }

        public OrderConfigurer<H> WithShippingAddresses(List<ShippingAddress> shippingAddresses)
        {
            foreach (ShippingAddress a in shippingAddresses)
            {
                a.ValidRequiredToCheckout();
            }
            GetOrNewShippingAddressList.AddRange(shippingAddresses);
            return this;
        }
        
        public OrderConfigurer<H> WithMonthlyInstallment(params MonthlyInstallment[] monthlyInstallment)
        {
            return WithMonthlyInstallment(new List<MonthlyInstallment>(monthlyInstallment));
        }

        public OrderConfigurer<H> WithMonthlyInstallment(List<MonthlyInstallment> monthlyInstallments)
        {
            foreach (MonthlyInstallment s in monthlyInstallments)
            {
                s.ValidRequired();
            }
            GetOrNewMonthlyInstallmentList.AddRange(monthlyInstallments);
            return this;
        }

        
        public OrderConfigurer<H> WithBankInfoList(List<Bank> bankInfoList)
        {
            GetOrNewBankInfoList.AddRange(bankInfoList);
            return this;
        }

        public OrderConfigurer<H> WithBankInfoList(params Bank[] bankInfoList)
        {
            WithBankInfoList(new List<Bank>(bankInfoList));
            return this;
        }

        public override string ClaimName()
        {
            return "checkoutInfo";
        }

        public override void ValidRequired()
        {
            if (productPrice <= 0)
                throw new IllegalArgumentException("product price field couldn't be zero. check yours input value : " + productPrice);
            if (Object.ReferenceEquals(null, productDeliveryInfo))
                throw new IllegalArgumentException("you should contain ProductDeliveryInfo object.");

            productDeliveryInfo.ValidRequired();

            if (offerList != null)
            {
                foreach (Offer o in offerList)
                {
                    o.ValidRequired();
                }
            }

            if (loyaltyList != null)
            {
                foreach (Loyalty l in loyaltyList)
                {
                    l.ValidRequired();
                }
            }

            if (shippingAddressList != null)
            {
                foreach (ShippingAddress a in shippingAddressList)
                {
                    a.ValidRequiredToCheckout();
                }
            }

            if (monthlyInstallmentList != null)
            {
                foreach (MonthlyInstallment m in monthlyInstallmentList)
                {
                    m.ValidRequired();
                }
            }
        }
    }
}

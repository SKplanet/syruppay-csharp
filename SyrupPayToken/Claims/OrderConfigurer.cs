using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using SyrupPayToken.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class OrderConfigurer<H> : AbstractTokenConfigurer<MerchantUserConfigurer<H>, H> where H : ITokenBuilder<H>
    {
        private int productPrice;
        private string submallName;
        private ProductDeliveryInfo productDeliveryInfo;
        private List<Offer> offerList = new List<Offer>();
        private List<Loyalty> loyaltyList = new List<Loyalty>();
        private List<PayConfigurer<H>.ShippingAddress> shippingAddressList = new List<PayConfigurer<H>.ShippingAddress>();
        private List<MonthlyInstallment> monthlyInstallmentList = new List<MonthlyInstallment>();

        public ReadOnlyCollection<MonthlyInstallment> GetMonthlyInstallmentList()
        {
            return monthlyInstallmentList.AsReadOnly();
        }

        public int GetProductPrice()
        {
            return productPrice;
        }

        public string GetSubmallName()
        {
            return submallName;
        }

        public ProductDeliveryInfo GetProductDeliveryInfo()
        {
            return productDeliveryInfo;
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

            foreach (Offer o in offerList)
            {
                o.ValidRequired();
            }
            foreach (Loyalty l in loyaltyList)
            {
                l.ValidRequired();
            }
            foreach (PayConfigurer<H>.ShippingAddress a in shippingAddressList)
            {
                a.ValidRequiredToCheckout();
            }
            foreach (MonthlyInstallment m in monthlyInstallmentList)
            {
                m.ValidRequired();
            }
        }

        public OrderConfigurer<H> WithShippingAddresses(params PayConfigurer<H>.ShippingAddress[] shippingAddress)
        {
            return WithShippingAddresses(new List<PayConfigurer<H>.ShippingAddress>(shippingAddress));
        }

        public OrderConfigurer<H> WithShippingAddresses(List<PayConfigurer<H>.ShippingAddress> shippingAddresses)
        {
            foreach (PayConfigurer<H>.ShippingAddress a in shippingAddresses)
            {
                a.ValidRequiredToCheckout();
            }
            shippingAddressList.AddRange(shippingAddresses);
            return this;
        }

        public OrderConfigurer<H> WithProductPrice(int productPrice)
        {
            if (productPrice <= 0)
            {
                throw new IllegalArgumentException("Cannot be smaller than 0. Check yours input value : " + productPrice);
            }
            this.productPrice = productPrice;
            return this;
        }

        public OrderConfigurer<H> WithSubmallName(string submallName)
        {
            this.submallName = submallName;
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
            this.offerList.AddRange(offers);
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
            this.loyaltyList.AddRange(loyalties);
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
            this.monthlyInstallmentList.AddRange(monthlyInstallments);
            return this;
        }

        public List<Offer> GetOfferList()
        {
            return offerList;
        }

        public List<Loyalty> GetLoyaltyList()
        {
            return loyaltyList;
        }

        public List<PayConfigurer<H>.ShippingAddress> GetShippingAddressList()
        {
            return shippingAddressList;
        }

        public enum LoyaltyId
        {
            [Description("www.11st.co.kr:point")]
            POINT_OF_11ST,
            [Description("www.11st.co.kr:mileage")]
            MILEAGE_OF_11ST,
            [Description("www.sktmemebership.co.kr")]
            T_MEMBERSHIP,
            [Description("www.okcashbag.com")]
            OK_CASHBAG
        }

        public enum ErrorType
        {
            NONE, MAINTENACE, SYSTEM_ERR
        }

        public enum OfferType
        {
            DELIVERY_COUPON
        }

        public enum AcceptType
        {
            NONE, CARD
        }

        public enum DeliveryType
        {
            NONE, PREPAID, FREE, DIY, QUICK, PAYMENT_ON_DELIVERY
        }

        interface Element
        {
            void ValidRequired();
        }

        public sealed class Accept : Element
        {
            [JsonConverter(typeof(StringEnumConverter))]
            private AcceptType type = AcceptType.NONE;
            private List<Dictionary<String, Object>> conditions = new List<Dictionary<String, Object>>();

            public new AcceptType GetType()
            {
                return type;
            }

            public Accept SetType(AcceptType type)
            {
                this.type = type;
                return this;
            }

            public List<Dictionary<String, Object>> GetConditions()
            {
                return conditions;
            }

            public Accept SetConditions(List<Dictionary<String, Object>> conditions)
            {
                this.conditions = conditions;
                return this;
            }

            public void ValidRequired()
            {
                if (type == AcceptType.NONE)
                {
                    throw new IllegalArgumentException("Accept object couldn't be with null fields.");
                }

                if (Object.ReferenceEquals(null, conditions))
                {
                    throw new IllegalArgumentException("Conditions of Accept object couldn't be empty. you should contain with conditions of Accept object.");
                }
            }
        }

        [JsonObject(MemberSerialization.Fields)]
        public sealed class ProductDeliveryInfo : Element
        {
            [JsonConverter(typeof(StringEnumConverter))]
            private DeliveryType deliveryType = DeliveryType.NONE;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            private string deliveryName;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            private bool defaultDeliveryCostApplied;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            private bool additionalDeliveryCostApplied;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            private bool shippingAddressDisplay;

            public bool IsShippingAddressDisplay()
            {
                return shippingAddressDisplay;
            }

            public ProductDeliveryInfo SetShippingAddressDisplay(bool shippingAddressDisplay)
            {
                this.shippingAddressDisplay = shippingAddressDisplay;
                return this;
            }

            public DeliveryType GetDeliveryType()
            {
                return deliveryType;
            }

            public ProductDeliveryInfo SetDeliveryType(DeliveryType deliveryType)
            {
                this.deliveryType = deliveryType;
                return this;
            }

            public string GetDeliveryName()
            {
                return deliveryName;
            }

            public ProductDeliveryInfo SetDeliveryName(string deliveryName)
            {
                this.deliveryName = deliveryName;
                return this;
            }

            public bool IsDefaultDeliveryCostApplied()
            {
                return defaultDeliveryCostApplied;
            }

            public ProductDeliveryInfo SetDefaultDeliveryCostApplied(bool defaultDeliveryCostApplied)
            {
                this.defaultDeliveryCostApplied = defaultDeliveryCostApplied;
                return this;
            }

            public bool IsAdditionalDeliveryCostApplied()
            {
                return additionalDeliveryCostApplied;
            }

            public ProductDeliveryInfo SetAdditionalDeliveryCostApplied(bool additionalDeliveryCostApplied)
            {
                this.additionalDeliveryCostApplied = additionalDeliveryCostApplied;
                return this;
            }

            public void ValidRequired()
            {
                if (deliveryType == DeliveryType.NONE || String.IsNullOrEmpty(deliveryName))
                {
                    throw new IllegalArgumentException("ProductDeliveryInfo object couldn't be with null fields. deliveryType : " + deliveryType + ", deliveryName : " + deliveryName);
                }
            }
        }

        public sealed class Offer : Element
        {
            private string id;
            private string userActionCode;
            [JsonConverter(typeof(StringEnumConverter))]
            private OfferType type;
            private string name;
            private int amountOff;
            private bool userSelectable;
            private int orderApplied;
            private string exclusiveGroupId;
            private string exclusiveGroupName;
            private List<Accept> accepted = new List<Accept>();

            public string GetUserActionCode()
            {
                return userActionCode;
            }

            public Offer SetUserActionCode(string userActionCode)
            {
                this.userActionCode = userActionCode;
                return this;
            }

            public Offer AddAcceptCardCondition(string cardCode, int minPaymentAmt)
            {
                Accept a = new Accept();
                a.SetType(AcceptType.CARD);
                Dictionary<String, Object> m = new Dictionary<String, Object>();
                m.Add("cardCode", cardCode);
                m.Add("minPaymentAmt", minPaymentAmt);
                a.GetConditions().Add(m);
                accepted.Add(a);
                return this;
            }

            public string GetExclusiveGroupId()
            {
                return exclusiveGroupId;
            }

            public Offer SetExclusiveGroupId(string exclusiveGroupId)
            {
                this.exclusiveGroupId = exclusiveGroupId;
                return this;
            }

            public string GetExclusiveGroupName()
            {
                return exclusiveGroupName;
            }

            public Offer SetExclusiveGroupName(string exclusiveGroupName)
            {
                this.exclusiveGroupName = exclusiveGroupName;
                return this;
            }

            public string GetId()
            {
                return id;
            }

            public Offer SetId(string id)
            {
                this.id = id;
                return this;
            }

            public new OfferType GetType()
            {
                return type;
            }

            public Offer SetType(OfferType type)
            {
                this.type = type;
                return this;
            }

            public string GetName()
            {
                return name;
            }

            public Offer SetName(string name)
            {
                this.name = name;
                return this;
            }

            public int GetAmountOff()
            {
                return amountOff;
            }

            public Offer SetAmountOff(int amountOff)
            {
                if (amountOff <= 0)
                {
                    throw new IllegalArgumentException("amountOff should be bigger than 0. yours : " + amountOff);
                }
                this.amountOff = amountOff;
                return this;
            }

            public bool IsUserSelectable()
            {
                return userSelectable;
            }

            public Offer SetUserSelectable(bool userSelectable)
            {
                this.userSelectable = userSelectable;
                return this;
            }

            public int GetOrderApplied()
            {
                return orderApplied;
            }

            public Offer SetOrderApplied(int orderApplied)
            {
                this.orderApplied = orderApplied;
                return this;
            }

            public List<Accept> GetAccepted()
            {
                return accepted;
            }

            public Offer SetAccepted(List<Accept> accepted)
            {
                this.accepted = accepted;
                return this;
            }

            public void ValidRequired()
            {
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(name))
                {
                    throw new IllegalArgumentException("Offer object couldn't be with null fields id : " + id + ", name : " + name);
                }
                if (amountOff <= 0)
                {
                    throw new IllegalArgumentException("amountOff field should be bigger than 0. yours amountOff is : " + amountOff);
                }
            }
        }

        public class AdditionalDiscount : Element
        {
            private int percentOff;
            private int maxApplicableAmt;

            public int GetPercentOff()
            {
                return percentOff;
            }

            public AdditionalDiscount SetPercentOff(int percentOff)
            {
                if (percentOff <= 0)
                {
                    throw new IllegalArgumentException("percentOff field should be bigger than 0. yours percentOff is : " + percentOff);
                }
                this.percentOff = percentOff;
                return this;
            }

            public int GetMaxApplicableAmt()
            {
                return maxApplicableAmt;
            }

            public AdditionalDiscount SetMaxApplicableAmt(int maxApplicableAmt)
            {
                if (maxApplicableAmt <= 0)
                {
                    throw new IllegalArgumentException("maxApplicableAmt field should be bigger than 0. yours maxApplicableAmt is : " + maxApplicableAmt);
                }
                this.maxApplicableAmt = maxApplicableAmt;
                return this;
            }

            public void ValidRequired()
            {
                if (percentOff <= 0)
                {
                    throw new IllegalArgumentException("percentOff field should be bigger than 0. yours percentOff is : " + percentOff);
                }
                if (maxApplicableAmt <= 0)
                {
                    throw new IllegalArgumentException("maxApplicableAmt field should be bigger than 0. yours maxApplicableAmt is : " + maxApplicableAmt);
                }
            }
        }

        public class Error : Element
        {
            [JsonConverter(typeof(StringEnumConverter))]
            private ErrorType type = ErrorType.NONE;
            private string description;

            public new ErrorType GetType()
            {
                return type;
            }

            public Error SetType(ErrorType type)
            {
                this.type = type;
                return this;
            }

            public string GetDescription()
            {
                return description;
            }

            public Error SetDescription(string description)
            {
                this.description = description;
                return this;
            }

            public void ValidRequired()
            {
                if (type == ErrorType.NONE || String.IsNullOrEmpty(description))
                {
                    throw new IllegalArgumentException("Error object couldn't be with null fields type : " + type + ", description : " + description);
                }
            }
        }

        public sealed class Loyalty : Element
        {
            private string id;
            private string userActionCode;
            private string name;
            private string subscriberId;
            private int balance;
            private int maxApplicableAmt;
            private int initialAppliedAmt;
            private int orderApplied;
            private AdditionalDiscount additionalDiscount;
            private Error error;
            private string exclusiveGroupId;
            private string exclusiveGroupName;

            public string GetUserActionCode()
            {
                return userActionCode;
            }

            public Loyalty SetUserActionCode(string userActionCode)
            {
                this.userActionCode = userActionCode;
                return this;
            }

            public string GetExclusiveGroupId()
            {
                return exclusiveGroupId;
            }

            public Loyalty SetExclusiveGroupId(string exclusiveGroupId)
            {
                this.exclusiveGroupId = exclusiveGroupId;
                return this;
            }

            public string GetExclusiveGroupName()
            {
                return exclusiveGroupName;
            }

            public Loyalty SetExclusiveGroupName(string exclusiveGroupName)
            {
                this.exclusiveGroupName = exclusiveGroupName;
                return this;
            }

            public string GetId()
            {
                return id;
            }

            public Loyalty SetId(string id)
            {
                this.id = id;
                return this;
            }

            public Loyalty SetIdBy(LoyaltyId loyaltyId)
            {
                this.id = EnumString<LoyaltyId>.GetValue(loyaltyId);
                return this;
            }

            public string GetName()
            {
                return name;
            }

            public Loyalty SetName(string name)
            {
                this.name = name;
                return this;
            }

            public string GetSubscriberId()
            {
                return subscriberId;
            }

            public Loyalty SetSubscriberId(string subscriberId)
            {
                this.subscriberId = subscriberId;
                return this;
            }

            public int GetBalance()
            {
                return balance;
            }

            public Loyalty SetBalance(int balance)
            {
                if (balance <= 0)
                {
                    throw new IllegalArgumentException("balance field should be bigger than 0. yours balance is : " + balance);
                }
                this.balance = balance;
                return this;
            }

            public int GetMaxApplicableAmt()
            {
                return maxApplicableAmt;
            }

            public Loyalty SetMaxApplicableAmt(int maxApplicableAmt)
            {
                if (maxApplicableAmt <= 0)
                {
                    throw new IllegalArgumentException("maxApplicableAmt field should be bigger than 0. yours maxApplicableAmt is : " + maxApplicableAmt);
                }
                this.maxApplicableAmt = maxApplicableAmt;
                return this;
            }

            public int GetInitialAppliedAmt()
            {
                return initialAppliedAmt;
            }

            public Loyalty SetInitialAppliedAmt(int initialAppliedAmt)
            {
                this.initialAppliedAmt = initialAppliedAmt;
                return this;
            }

            public int GetOrderApplied()
            {
                return orderApplied;
            }

            public Loyalty SetOrderApplied(int orderApplied)
            {
                this.orderApplied = orderApplied;
                return this;
            }

            public AdditionalDiscount GetAdditionalDiscount()
            {
                return additionalDiscount;
            }

            public Loyalty SetAdditionalDiscount(AdditionalDiscount additionalDiscount)
            {
                this.additionalDiscount = additionalDiscount;
                return this;

            }

            public Error GetError()
            {
                return error;
            }

            public Loyalty SetError(Error error)
            {
                this.error = error;
                return this;
            }

            public void ValidRequired()
            {
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(name) || String.IsNullOrEmpty(subscriberId))
                {
                    throw new IllegalArgumentException("Loyalty object couldn't be with null fields id : " + id + ", name : " + name + ", subscriberId : " + subscriberId);
                }

                if (!Object.ReferenceEquals(null, additionalDiscount))
                {
                    additionalDiscount.ValidRequired();
                }

                if (!Object.ReferenceEquals(null, error))
                {
                    error.ValidRequired();
                }

                if (balance <= 0)
                {
                    throw new IllegalArgumentException("balance field should be bigger than 0. yours balance is : " + balance);
                }
                if (maxApplicableAmt <= 0)
                {
                    throw new IllegalArgumentException("maxApplicableAmt field should be bigger than 0. yours maxApplicableAmt is : " + maxApplicableAmt);
                }
            }
        }

        public class MonthlyInstallment : Element
        {
            private string cardCode;
            private List<Dictionary<string, Object>> conditions = new List<Dictionary<string, Object>>();

            public string GetCardCode()
            {
                return cardCode;
            }

            public MonthlyInstallment SetCardCode(string cardCode)
            {
                this.cardCode = cardCode;
                return this;
            }

            public List<Dictionary<string, Object>> GetConditions()
            {
                return conditions;
            }

            public MonthlyInstallment AddCondition(int min, bool includeMin, int max, bool includeMax, string monthlyInstallmentInfo)
            {
                Dictionary<string, Object> m = new Dictionary<string, Object>();
                m.Add("paymentAmtRange", (includeMin ? "[" : "(") + min + "-" + max + (includeMax ? "]" : ")"));
                m.Add("monthlyInstallmentInfo", monthlyInstallmentInfo);
                this.conditions.Add(m);
                return this;
            }

            public MonthlyInstallment AddCondition(int min, bool includeMin, string monthlyInstallmentInfo)
            {
                Dictionary<string, Object> m = new Dictionary<string, Object>();
                m.Add("paymentAmtRange", (includeMin ? "[" : "(") + min + "-]");
                m.Add("monthlyInstallmentInfo", monthlyInstallmentInfo);
                this.conditions.Add(m);
                return this;
            }

            public void ValidRequired()
            {
                if (String.IsNullOrEmpty(cardCode))
                {
                    throw new IllegalArgumentException("MonthlyInstallment object couldn't be with null fields cardCode is null");
                }

                if (Object.ReferenceEquals(null, conditions))
                {
                    throw new IllegalArgumentException("Conditions of MonthlyInstallment object couldn't be empty. you should contain with conditions of MonthlyInstallment object by addCondition method.");
                }
            }
        }
    }
}

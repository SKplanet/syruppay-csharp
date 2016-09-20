using System;
using SyrupPayToken.Claims;
using Newtonsoft.Json;
using SyrupPayToken.Utils;

namespace SyrupPayToken.jwt
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class SyrupPayToken<H> : IToken<H> where H : ITokenBuilder<H>
    {
        private String aud = "https://pay.syrup.co.kr";
        private String typ = "jose";
        private String iss = null;
        private long exp = 0;
        private long iat = 0;
        private String jti = null;
        private long nbf = 0;
        private String sub = null;

        private MerchantUserConfigurer<H> loginInfo = null;
        private PayConfigurer<H> transactionInfo = null;
        private MapToSyrupPayUserConfigurer<H> userInfoMapper = null;
        private OrderConfigurer<H> checkoutInfo = null;
        private SubscriptionConfigurer<H> subscription = null;

        public MerchantUserConfigurer<H> LoginInfo
        {
            get { return GetLoginInfo(); }
        }

        public PayConfigurer<H> TransactionInfo
        {
            get { return GetTransactionInfo(); }
        }

        public MapToSyrupPayUserConfigurer<H> UserInfoMapper
        {
            get { return GetUserInfoMapper();}
        }

        public OrderConfigurer<H> CheckoutInfo
        {
            get { return GetCheckoutInfo(); }
        }

        public SubscriptionConfigurer<H> Subscription
        {
            get { return GetSubscription(); }
        }

        public string Iss
        {
            get { return GetIss(); }
        }

        public string Sub
        {
            get { return GetSub(); }
        }

        public string Aud
        {
            get { return GetAud(); }
        }

        public long Exp
        {
            get { return GetExp(); }
        }

        public long Nbf
        {
            get { return GetNbf(); }
        }

        public long Iat
        {
            get { return GetIat(); }
        }

        public string Jti
        {
            get { return GetJti(); }
        }

        public OrderConfigurer<H> GetCheckoutInfo()
        {
            return checkoutInfo;
        }

        public bool IsValidInTimes()
        {
            long ticks = new DateTime(1970, 1, 1).Ticks;

            DateTime nbf = new DateTime(ticks);
            nbf.AddMilliseconds(this.nbf * 1000);

            DateTime exp = new DateTime(ticks);
            exp.AddMilliseconds(this.exp * 1000);

            return (this.nbf <= 0 || DateTime.Now > nbf) && DateTime.Now < exp;
        }

        public String GetAud()
        {
            return aud;
        }

        public String GetTyp()
        {
            return typ;
        }

        public String GetIss()
        {
            return iss;
        }

        public long GetExp()
        {
            return exp;
        }

        public long GetIat()
        {
            return iat;
        }

        public String GetJti()
        {
            return jti;
        }

        public long GetNbf()
        {
            return nbf;
        }

        public String GetSub()
        {
            return sub;
        }

        public MerchantUserConfigurer<H> GetLoginInfo()
        {
            return loginInfo;
        }

        public PayConfigurer<H> GetTransactionInfo()
        {
            if (Object.ReferenceEquals(null, transactionInfo))
            {
                transactionInfo = new PayConfigurer<H>();
            }
            return transactionInfo;
        }

        [Obsolete("set only mctTransAuthId of transactionInfo element by deprecated method", false)]
        public void SetTransactionInfo(PayConfigurer<H> transactionInfo)
        {
            if (!Object.ReferenceEquals(null, this.transactionInfo) && 
                String.IsNullOrEmpty(this.transactionInfo.MctTransAuthId) && 
                String.IsNullOrEmpty(transactionInfo.PaymentInfo.ProductTitle))
            {
                transactionInfo.WithOrderIdOfMerchant(transactionInfo.MctTransAuthId);
            }
            else
            {
                this.transactionInfo = transactionInfo;
            }
        }

        public MapToSyrupPayUserConfigurer<H> GetUserInfoMapper()
        {
            return userInfoMapper;
        }

        public SubscriptionConfigurer<H> GetSubscription()
        {
            return subscription;
        }

        [Obsolete("set paymentInfo element by deprecated method", false)]
        public void setPaymentInfo(PaymentInformationBySeller paymentInfo)
        {
            GetTransactionInfo().WithAmount(paymentInfo.PaymentAmt);
            GetTransactionInfo().WithLanguageForDisplay((Language) Enum.Parse(typeof(Language), paymentInfo.Lang.ToUpper()));
            GetTransactionInfo().WithShippingAddress(paymentInfo.ShippingAddress);
            GetTransactionInfo().WithProductTitle(paymentInfo.ProductTitle);
            GetTransactionInfo().WithProductUrls(paymentInfo.ProductUrls);
            GetTransactionInfo().WithCurrency((Currency)Enum.Parse(typeof(Language), paymentInfo.CurrencyCode.ToUpper()));
            GetTransactionInfo().WithDeliveryName(paymentInfo.DeliveryName);
            GetTransactionInfo().WithDeliveryPhoneNumber(paymentInfo.DeliveryPhoneNumber);
            GetTransactionInfo().WithInstallmentPerCardInformation(paymentInfo.CardInfoList);
        }

        [Obsolete("set paymentRestrictions element by deprecated method", false)]
        public void setPaymentRestrictions(PaymentRestriction paymentRestriction)
        {
            foreach (PayableLocaleRule r in Enum.GetValues(typeof(PayableLocaleRule)))
            {
                if (String.Equals(EnumString<PayableLocaleRule>.GetValue(r), paymentRestriction.CardIssuerRegion))
                {
                    GetTransactionInfo().WithPayableRuleWithCard(r);
                }
            }
        }
    }
}

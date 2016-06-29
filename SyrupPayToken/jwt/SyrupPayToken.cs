using System;
using System.Collections.Generic;
using System.Text;
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
        private MapToSktUserConfigurer<H> lineInfo = null;
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

        public MapToSktUserConfigurer<H> LineInfo
        {
            get { return GetLineInfo(); }
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
                String.IsNullOrEmpty(this.transactionInfo.GetMctTransAuthId()) && 
                String.IsNullOrEmpty(transactionInfo.GetPaymentInfo().ProductTitle))
            {
                transactionInfo.WithOrderIdOfMerchant(transactionInfo.GetMctTransAuthId());
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

        public MapToSktUserConfigurer<H> GetLineInfo()
        {
            return lineInfo;
        }

        public SubscriptionConfigurer<H> GetSubscription()
        {
            return subscription;
        }

        [Obsolete("set paymentInfo element by deprecated method", false)]
        public void setPaymentInfo(PayConfigurer<H>.PaymentInformationBySeller paymentInfo)
        {
            GetTransactionInfo().WithAmount(paymentInfo.GetPaymentAmt());
            GetTransactionInfo().WithLanguageForDisplay((PayConfigurer<H>.Language) Enum.Parse(typeof(PayConfigurer<H>.Language), paymentInfo.GetLang().ToUpper()));
            GetTransactionInfo().WithShippingAddress(paymentInfo.GetShippingAddress());
            GetTransactionInfo().WithProductTitle(paymentInfo.GetProductTitle());
            GetTransactionInfo().WithProductUrls(paymentInfo.GetProductUrls());
            GetTransactionInfo().WithCurrency((PayConfigurer<H>.Currency)Enum.Parse(typeof(PayConfigurer<H>.Language), paymentInfo.GetCurrencyCode().ToUpper()));
            GetTransactionInfo().WithDeliveryName(paymentInfo.GetDeliveryName());
            GetTransactionInfo().WithDeliveryPhoneNumber(paymentInfo.GetDeliveryPhoneNumber());
            GetTransactionInfo().WithInstallmentPerCardInformation(paymentInfo.GetCardInfoList());
        }

        [Obsolete("set paymentRestrictions element by deprecated method", false)]
        public void setPaymentRestrictions(PayConfigurer<H>.PaymentRestriction paymentRestriction)
        {
            foreach (PayConfigurer<H>.PayableLocaleRule r in Enum.GetValues(typeof(PayConfigurer<H>.PayableLocaleRule)))
            {
                if (String.Equals(EnumString<PayConfigurer<H>.PayableLocaleRule>.GetValue(r), paymentRestriction.CardIssuerRegion))
                {
                    GetTransactionInfo().WithPayableRuleWithCard(r);
                }
            }
        }
    }
}

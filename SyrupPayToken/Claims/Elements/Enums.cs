using SyrupPayToken.Utils;

namespace SyrupPayToken
{
    public enum MappingType
    {
        UNDEFINED, CI_HASH, CI_MAPPED_KEY
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
        UNDEFINED, MAINTENACE, SYSTEM_ERR
    }

    public enum OfferType
    {
        DELIVERY_COUPON
    }

    public enum AcceptType
    {
        UNDEFINED, CARD, BANK, MOBILE, SYRUP_PAY_COUPON
    }

    public enum CashReceiptDisplay
    {
        YES, NO, DELEGATE_ADMIN
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

    public enum SubscriptionInterval
    {
        [Description("ONDEMAND")]
        ONDEMAND,
        [Description("MONTHLY")]
        MONTHLY,
        [Description("WEEKLY")]
        WEEKLY,
        [Description("BIWEEKLY")]
        BIWEEKLY
    }

    public enum MatchedUser
    {
        [Description("CI_MATCHED_ONLY")]
        CI_MATCHED_ONLY
    }


    public enum DeliveryType
    {
        UNDEFINED, PREPAID, FREE, DIY, QUICK, PAYMENT_ON_DELIVERY
    }
}

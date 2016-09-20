using SyrupPayToken.Claims;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyrupPayToken.jwt
{
    public interface IToken<H> : IJwtToken where H : ITokenBuilder<H>
    {
        MerchantUserConfigurer<H> LoginInfo { get; }
        MerchantUserConfigurer<H> GetLoginInfo();

        PayConfigurer<H> TransactionInfo { get; }
        PayConfigurer<H> GetTransactionInfo();

        MapToSyrupPayUserConfigurer<H> UserInfoMapper { get; }
        MapToSyrupPayUserConfigurer<H> GetUserInfoMapper();

        bool IsValidInTimes();

        OrderConfigurer<H> CheckoutInfo { get; }
        OrderConfigurer<H> GetCheckoutInfo();

        SubscriptionConfigurer<H> Subscription { get; }
        SubscriptionConfigurer<H> GetSubscription();
    }
}

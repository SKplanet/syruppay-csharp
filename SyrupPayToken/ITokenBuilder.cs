using System;

namespace SyrupPayToken
{
    public interface ITokenBuilder<H> : IClaimBuilder<Jwt> where H : ITokenBuilder<H>
    {
        C GetConfigurer<C>(Type type) where C : IClaimConfigurer<Jwt, H>;
        C RemoveConfigurer<C>(Type type) where C : IClaimConfigurer<Jwt, H>;
    }
}

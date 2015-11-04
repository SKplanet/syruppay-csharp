namespace SyrupPayToken.Claims
{
    public abstract class AbstractTokenConfigurer<T, B> : ClaimConfigurerAdapter<Jwt, B>
        where T : AbstractTokenConfigurer<T, B>
        where B : ITokenBuilder<B>
    {
        public B Disable()
        {
            GetBuilder().RemoveConfigurer<IClaimConfigurer<Jwt, B>>(this.GetType());
            return GetBuilder();
        }
    }
}

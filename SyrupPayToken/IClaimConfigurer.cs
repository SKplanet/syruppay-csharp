namespace SyrupPayToken
{
    public interface IClaimConfigurer<O, B> where B : IClaimBuilder<O>
    {
        string ClaimName();
        void Init(B builder);
        void Configure(B builder);
        void ValidRequired();
    }
}

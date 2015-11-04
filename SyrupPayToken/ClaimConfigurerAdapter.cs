using System;

namespace SyrupPayToken
{
    public abstract class ClaimConfigurerAdapter<O, B> : IClaimConfigurer<O, B> where B : IClaimBuilder<O>
    {
        [NonSerialized()]
        private B builder;

        public void Init(B builder)
        {
        }

        public void Configure(B builder)
        {
        }

        public B And()
        {
            return GetBuilder();
        }

        protected B GetBuilder()
        {
            if (Object.ReferenceEquals(null, builder))
            {
                throw new NullReferenceException("builder cannot be null");
            }
            return builder;
        }

        public void SetBuilder(B builder)
        {
            this.builder = builder;
        }

        public abstract string ClaimName();
        public abstract void ValidRequired();
    }
}

using SyrupPayToken.exception;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace SyrupPayToken
{
    public abstract class AbstractConfiguredTokenBuilder<O, B> : AbstractClaimBuilder<O> where B : IClaimBuilder<O>
    {
        private OrderedDictionary configurers = new OrderedDictionary();

        public C GetConfigurer<C>(Type type) where C : IClaimConfigurer<O, B>
        {
            List<IClaimConfigurer<O, B>> configs = (List < IClaimConfigurer < O, B>>) this.configurers[type];
            if (Object.ReferenceEquals(null, configs))
            {
                return default(C);
            }
            if (configs.Count != 1)
            {
                throw new IllegalStateException("Only one configurer expected for type " + type + ", but got " + configs);
            }

            return (C)configs[0];
        }

        public C RemoveConfigurer<C>(Type type) where C : IClaimConfigurer<O, B>
        {
            List<IClaimConfigurer<O, B>> configs = (List < IClaimConfigurer < O, B>>) this.configurers[type];
            this.configurers.Remove(type);

            if (Object.ReferenceEquals(null, configs))
            {
                return default(C);
            }
            if (configs.Count != 1)
            {
                throw new IllegalStateException("Only one configurer expected for type " + type + ", but got " + configs);
            }
            return (C)configs[0];
        }

        public C Apply<C>(C configurer) where C : ClaimConfigurerAdapter<O, B>
        {
            Add(configurer);
            Object obj = this;
            configurer.SetBuilder((B)obj);
            return configurer;
        }

        private void Add<C>(C configurer) where C : IClaimConfigurer<O, B>
        {
            Type type = configurer.GetType();
            List<IClaimConfigurer<O, B>> configs = (List < IClaimConfigurer < O, B>> ) this.configurers[type];
            if (Object.ReferenceEquals(null, configs))
            {
                configs = new List<IClaimConfigurer<O, B>>();
            }
            configs.Add(configurer);
            this.configurers.Add(type, configs);
        }

        public Type[] GetClasses()
        {
            Type[] types = new Type[configurers.Count];
            configurers.Keys.CopyTo(types, 0);

            return types;
        }
    }
}

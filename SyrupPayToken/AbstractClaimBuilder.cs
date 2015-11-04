using SyrupPayToken.exception;
using System;

namespace SyrupPayToken
{
    public abstract class AbstractClaimBuilder<O> : IClaimBuilder<O>
    {
        private bool building = false;
        private O obj;
        private readonly Object locObj = new Object();

        public O Build()
        {
            lock(locObj)
            {
                if (!building)
                {
                    obj = DoBuild();
                    building = true;
                    return obj;
                }

                throw new AlreadyBuiltException("This object has already been built");
            }
        }

        public O GetObject()
        {
            if (!building)
            {
                throw new NullReferenceException("This object has not been built");
            }
            return obj;
        }

        protected abstract O DoBuild();
    }
}

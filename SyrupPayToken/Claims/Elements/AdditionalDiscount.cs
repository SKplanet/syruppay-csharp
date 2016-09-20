using Newtonsoft.Json;
using SyrupPayToken.exception;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public class AdditionalDiscount : Element
    {
        private double percentOff;
        private int maxApplicableAmt;

        public AdditionalDiscount SetPercentOff(double percentOff)
        {
            this.percentOff = percentOff;
            return this;
        }

        public AdditionalDiscount SetMaxApplicableAmt(int maxApplicableAmt)
        {
            this.maxApplicableAmt = maxApplicableAmt;
            return this;
        }

        public void ValidRequired()
        {
            if (percentOff <= 0)
            {
                throw new IllegalArgumentException("percentOff field should be bigger than 0. yours percentOff is : " + percentOff);
            }
            if (maxApplicableAmt <= 0)
            {
                throw new IllegalArgumentException("maxApplicableAmt field should be bigger than 0. yours maxApplicableAmt is : " + maxApplicableAmt);
            }
        }
    }
}

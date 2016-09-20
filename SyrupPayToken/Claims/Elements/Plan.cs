using System;
using Newtonsoft.Json;
using SyrupPayToken.exception;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class Plan : Element
    {
        private string interval;
        private string name;

        public string Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public void ValidRequired()
        {
            if (interval == null)
                throw new IllegalArgumentException("Plan 'interval' should not be null.");

            if (name == null)
                throw new IllegalArgumentException("Plan 'name' should not be null.");
        }
    }
}

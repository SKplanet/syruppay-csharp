using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using System;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public class Error : Element
    {
        [JsonConverter(typeof(StringEnumConverter))]
        private Nullable<ErrorType> type = null;
        private string description;

        public Error SetType(ErrorType type)
        {
            this.type = type;
            return this;
        }

        public Error SetDescription(string description)
        {
            this.description = description;
            return this;
        }

        public void ValidRequired()
        {
            if (type.GetValueOrDefault() == ErrorType.UNDEFINED || String.IsNullOrEmpty(description))
            {
                throw new IllegalArgumentException("Error object couldn't be with null fields type : " + type + ", description : " + description);
            }
        }
    }
}

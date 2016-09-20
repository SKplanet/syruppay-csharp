using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using System;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public class MapToSyrupPayUserConfigurer<H> : AbstractTokenConfigurer<MapToSyrupPayUserConfigurer<H>, H> where H : ITokenBuilder<H>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        private MappingType mappingType = MappingType.UNDEFINED;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string mappingValue;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private string identityAuthenticationId;

        public MapToSyrupPayUserConfigurer<H> WithType(MappingType type)
        {
            this.mappingType = type;
            return this;
        }

        public MapToSyrupPayUserConfigurer<H> WithValue(string value)
        {
            this.mappingValue = value;
            return this;
        }

        public MapToSyrupPayUserConfigurer<H> WithIdentityAuthenticationId(string identityAuthenticationId)
        {
            this.identityAuthenticationId = identityAuthenticationId;
            return this;
        }

        public override string ClaimName()
        {
            return "userInfoMapper";
        }

        public override void ValidRequired()
        {
            if (mappingType == MappingType.UNDEFINED || String.IsNullOrEmpty(mappingValue))
            {
                throw new IllegalArgumentException("fields to map couldn't be null. type : " + this.mappingType + "value : " + this.mappingValue);
            }
        }
    }
}

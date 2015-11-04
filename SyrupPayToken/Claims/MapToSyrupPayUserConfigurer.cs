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
        private MappingType mappingType = MappingType.NONE;
        private string mappingValue;

        public MappingType GetMappingType()
        {
            return mappingType;
        }

        public string GetMappingValue()
        {
            return mappingValue;
        }

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

        public override string ClaimName()
        {
            return "userInfoMapper";
        }

        public override void ValidRequired()
        {
            if (mappingType == MappingType.NONE || String.IsNullOrEmpty(mappingValue))
            {
                throw new IllegalArgumentException("fields to map couldn't be null. type : " + this.mappingType + "value : " + this.mappingValue);
            }
        }

        public enum MappingType
        {
            NONE, CI_HASH, CI_MAPPED_KEY
        }
    }
}

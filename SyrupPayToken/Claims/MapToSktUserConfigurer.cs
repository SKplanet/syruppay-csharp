using Newtonsoft.Json;
using SyrupPayToken.exception;
using System;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public class MapToSktUserConfigurer<H> : AbstractTokenConfigurer<MapToSktUserConfigurer<H>, H> where H : ITokenBuilder<H>
    {
        private string lineNumber;
        private string svcMgmtNumber;

        public override string ClaimName()
        {
            return "lineInfo";
        }

        public override void ValidRequired()
        {
            if (Object.ReferenceEquals(null, lineNumber))
            {
                throw new IllegalArgumentException("line number cannot be null");
            }

            if (lineNumber.Contains("-"))
            {
                throw new IllegalArgumentException("line number should be without '-' mark. ex) 01011112222");
            }
        }

        public string GetLineNumber()
        {
            return lineNumber;
        }

        public string GetSvcMgmtNumber()
        {
            return svcMgmtNumber;
        }

        public MapToSktUserConfigurer<H> WithLineNumber(string lineNumber)
        {
            this.lineNumber = lineNumber;
            return this;
        }

        public MapToSktUserConfigurer<H> WithServiceManagementNumber(string serviceManagementNumber)
        {
            this.svcMgmtNumber = serviceManagementNumber;
            return this;
        }
    }
}

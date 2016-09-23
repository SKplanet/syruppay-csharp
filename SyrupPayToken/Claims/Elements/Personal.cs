using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using System;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class Personal
    {
        private string username;
        private string lineNumber;
        [JsonConverter(typeof(StringEnumConverter))]
        private OperatorCode operatorCode;
        private string ssnFirst7Digit;
        private string email;
        private string ciHash;
        private PayableCard payableCard;

        public Personal(string username, string ssnFirst7Digit, string lineNumber)
        {
            if (username == null || ssnFirst7Digit == null || lineNumber == null)
            {
                throw new IllegalArgumentException(String.Format("you should set with valid parameters to create this instance. username: {1}, ssnFirst7Digit: {2}, lineNumber: {3}", username, ssnFirst7Digit, lineNumber));
            }
            else if (ssnFirst7Digit.Length != 7)
            {
                throw new IllegalArgumentException(String.Format("length of ssnFirst7Digit should be 7. yours: {1} ({2})", ssnFirst7Digit, ssnFirst7Digit.Length));
            }
            this.username = username;
            this.ssnFirst7Digit = ssnFirst7Digit;
            this.lineNumber = lineNumber;
        }

        public Personal() { }
        public Personal SetUserName(string username)
        {
            if (username == null || username.Length == 0)
            {
                throw new IllegalArgumentException("username shouldn't be null and not empty.");
            }
            this.username = username;
            return this;
        }

        public Personal SetSsnFirst7Digit(string ssnFirst7Digit)
        {
            if (ssnFirst7Digit == null || ssnFirst7Digit.Length == 0)
            {
                throw new IllegalArgumentException("ssnFirst7Digit shouldn't be null and not empty.");
            }

            if (ssnFirst7Digit.Length != 7)
            {
                throw new IllegalArgumentException(String.Format("length of ssnFirst7Digit should be 7. yours inputs is : {1}", ssnFirst7Digit));

            }
            this.ssnFirst7Digit = ssnFirst7Digit;
            return this;
        }

        public Personal SetOperatorCode(OperatorCode operatorCode)
        {
            this.operatorCode = operatorCode;
            return this;
        }

        public Personal SetLineNumber(string lineNumber)
        {
            if (lineNumber == null || lineNumber.Length == 0)
            {
                throw new IllegalArgumentException("lineNumber shouldn't be null and not empty.");
            }
            this.lineNumber = lineNumber;
            return this;
        }

        public Personal SetEmail(string email)
        {
            this.email = email;
            return this;
        }

        public Personal SetCiHash(string ciHash)
        {
            this.ciHash = ciHash;
            return this;
        }

        public Personal SetPayableCard(PayableCard payableCard)
        {
            this.payableCard = payableCard;
            return this;
        }
    }
}

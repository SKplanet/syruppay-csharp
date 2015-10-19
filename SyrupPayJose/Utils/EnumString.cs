using SyrupPayJose.Jwa;
using System;
using System.Reflection;

namespace SyrupPayJose.Utils
{
    public static class EnumString
    {
        public static string GetValue(JsonWebAlgorithm value)
        {
            string output = null;

            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            Description[] attrs = fi.GetCustomAttributes(typeof(Description), false) as Description[];

            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        public static JsonWebAlgorithm GetEnum(string enumName)
        {
            enumName = enumName.Replace("-", "_");
            return (JsonWebAlgorithm)Enum.Parse(typeof(JsonWebAlgorithm), enumName);
        }
    }
}

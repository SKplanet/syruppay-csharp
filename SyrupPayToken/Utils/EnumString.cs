using SyrupPayToken.exception;
using System;
using System.Reflection;

namespace SyrupPayToken.Utils
{
    public class EnumString<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        public static string GetValue(T value)
        {
            string output = null;
            
            if (!typeof(T).IsEnum)
            {
                throw new InvalidTypeException("Parameter(value) is should be Enum value.");
            }

            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            Description[] attrs = fi.GetCustomAttributes(typeof(Description), false) as Description[];

            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        public static T GetEnum(string enumName)
        {
            return (T)Enum.Parse(typeof(T), enumName);
        }
    }
}

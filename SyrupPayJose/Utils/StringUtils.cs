using System.Text;

namespace SyrupPayJose.Utils
{
    public class StringUtils
    {
        // 바이트 배열을 String으로 변환 
        public static string ByteToString(byte[] strByte)
        {
            return Encoding.UTF8.GetString(strByte);
        }
        // String을 바이트 배열로 변환 
        public static byte[] StringToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string toHex(byte[] b)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte a in b)
            {
                sb.AppendFormat("{0:x2}", a);
            }

            return sb.ToString();
        }
    }
}

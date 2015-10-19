using SyrupPayJose.Utils;

namespace SyrupPayJose.Jwa
{
    public enum JsonWebAlgorithm
    {
        [Description("NONE")]
        NONE = 0,
        [Description("A128KW")]
        A128KW = 1,
        [Description("A256KW")]
        A256KW,
        [Description("A128CBC-HS256")]
        A128CBC_HS256,
        [Description("HS256")]
        HS256
    }
}

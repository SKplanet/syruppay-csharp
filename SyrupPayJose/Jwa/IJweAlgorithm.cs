using SyrupPayJose.Jwa.Alg;
using SyrupPayJose.Jwa.Enc;

namespace SyrupPayJose.Jwa
{
    public interface IJweAlgorithm
    {
        JwaAlgResult Encryption(byte[] key, ContentEncryptKeyGenerator cekGenerator);
        byte[] Decryption(byte[] key, byte[] src);
    }
}

using SyrupPayJose.Jwa.Enc;

namespace SyrupPayJose.Jwa.Alg
{
    public struct JwaAlgResult
    {
        public byte[] cek;
        public byte[] encryptedCek;
    }

    public abstract class KeyEncryption : IJweAlgorithm
    {
        abstract public byte[] Decryption(byte[] key, byte[] src);
        abstract public JwaAlgResult Encryption(byte[] key, ContentEncryptKeyGenerator cekGenerator);
    }
}

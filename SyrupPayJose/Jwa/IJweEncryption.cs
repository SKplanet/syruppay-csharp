using SyrupPayJose.Jwa.Enc;

namespace SyrupPayJose.Jwa
{
    public interface IJweEncryption
    {
        JweEncResult EncryptionAndSign(byte[] key, byte[] iv, byte[] src, byte[] aad);
        byte[] VerifyAndDecryption(byte[] key, byte[] iv, byte[] src, byte[] aad, byte[] expected);
        byte[] GenerateRandomIv();
        ContentEncryptKeyGenerator getContentEncryptionKeyGenerator();
    }
}

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace SyrupPayJose.Jwa.Alg
{
    public class Aes128KeyWrap : AesKeyWrap
    {
        public Aes128KeyWrap(int keyLength) : base(keyLength) { }

        public override JwaAlgResult Wrap(byte[] key, byte[] src)
        {
            var engine = new AesWrapEngine();
            engine.Init(true, new KeyParameter(key));

            JwaAlgResult jwaAlgResult;
            jwaAlgResult.cek = src;
            jwaAlgResult.encryptedCek = engine.Wrap(src, 0, src.Length);

            return jwaAlgResult;
        }

        public override byte[] UnWrap(byte[] key, byte[] src)
        {
            var engine = new AesWrapEngine();
            engine.Init(false, new KeyParameter(key));
            return engine.Unwrap(src, 0, src.Length);
        }
    }
}

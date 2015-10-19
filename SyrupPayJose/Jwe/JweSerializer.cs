using SyrupPayJose.Utils;
using SyrupPayJose.Jwa;
using System;
using SyrupPayJose.Jwa.Enc;
using SyrupPayJose.Jwa.Alg;

namespace SyrupPayJose.Jwe
{
    struct JweStructure
    {
        public JoseHeader joseHeader;
        public byte[] cek;
        public byte[] iv;
        public byte[] cipherText;
        public byte[] at;

        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}.{3}.{4}",
                joseHeader.GetSerialize(),
                Base64.base64urlencode(cek),
                Base64.base64urlencode(iv),
                Base64.base64urlencode(cipherText),
                Base64.base64urlencode(at));
        }

        public void Set(string src)
        {
            var token = src.Split('.');
            if (token == null || token.Length != 5)
            {
                throw new IllegalEncryptionTokenException();
            }

            joseHeader = new JoseHeader();
            joseHeader.SetSerialize(token[0]);
            cek = Base64.base64urldecode(token[1]);
            iv = Base64.base64urldecode(token[2]);
            cipherText = Base64.base64urldecode(token[3]);
            at = Base64.base64urldecode(token[4]);
        }
    }

    public class JweSerializer : IJoseAction
    {
        private string payload;
        private byte[] key;

        private JweStructure jweStructure;

        public JweSerializer(JoseHeader header, string payload, string key)
        {
            this.jweStructure.joseHeader = header;
            this.payload = payload;
            this.key = StringUtils.StringToByte(key);
        }

        public JweSerializer(String serialized, string key)
        {
            this.jweStructure.Set(serialized);
            this.key = StringUtils.StringToByte(key);
        }

        public JoseHeader GetHeader()
        {
            return jweStructure.joseHeader;
        }

        public void SetUserEncryptionKey(byte[] cek, byte[] iv)
        {
            jweStructure.cek = cek;
            jweStructure.iv = iv;
        }

        public byte[] GetAad()
        {
            return StringUtils.StringToByte(jweStructure.joseHeader.GetSerialize());
        }

        public string CompactSerialization()
        {
            IJweAlgorithm jweAlgorithm = JwaFactory.GetJweAlgorithm(jweStructure.joseHeader.Alg);
            IJweEncryption jweEncryption = JwaFactory.GetJweEncryption(jweStructure.joseHeader.Enc);

            ContentEncryptKeyGenerator cekGenerator = jweEncryption.getContentEncryptionKeyGenerator();
            cekGenerator.UserEncryptionKey = jweStructure.cek;

            JwaAlgResult jwaAlgResult = jweAlgorithm.Encryption(key, cekGenerator);
            jweStructure.cek = jwaAlgResult.encryptedCek;

            var jweEncResult = jweEncryption.EncryptionAndSign(jwaAlgResult.cek,
                jweStructure.iv,
                StringUtils.StringToByte(payload),
                GetAad());

            jweStructure.cipherText = jweEncResult.cipherText;
            jweStructure.at = jweEncResult.at;
            jweStructure.iv = jweEncResult.iv;

            return jweStructure.ToString();
        }

        public string CompactDeserialization()
        {
            IJweAlgorithm jweAlgorithm = JwaFactory.GetJweAlgorithm(jweStructure.joseHeader.Alg);
            IJweEncryption jweEncryption = JwaFactory.GetJweEncryption(jweStructure.joseHeader.Enc);

            var cek = jweAlgorithm.Decryption(key, jweStructure.cek);
            var payload = jweEncryption.VerifyAndDecryption(cek,
                jweStructure.iv,
                jweStructure.cipherText,
                GetAad(),
                jweStructure.at);

            return StringUtils.ByteToString(payload);
        }
    }
}

using System;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

namespace SyrupPayJose.Jwa.Alg
{

    /// <summary>
    /// https://github.com/dvsekhvalnov/jose-jwt
    /// </summary>
    class AesWrapEngine
    {
        private readonly byte[] DefaultIV = { 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6 };
        private byte[] kek = null;

        internal void Init(byte[] kek)
        {
            this.kek = kek;
        }

        internal byte[] Wrap(byte[] cek)
        {
            // 1) Initialize variables
            byte[] a = DefaultIV;                       // Set A = IV, an initial value
            byte[][] r = Arrays.Slice(cek, 8);          // For i = 1 to n
            //     R[0][i] = P[i]
            long n = r.Length;
            // 2) Calculate intermediate values.
            for (long j = 0; j < 6; j++)                                      // For j = 0 to 5
            {
                for (long i = 0; i < n; i++)                                  //    For i=1 to n
                {
                    long t = n * j + i + 1;

                    byte[] b = AesEnc(kek, Arrays.Concat(a, r[i]));     //      B=AES(K, A | R[i])
                    a = Arrays.FirstHalf(b);                                  //      A=MSB(64,B) ^ t where t = (n*j)+i
                    r[i] = Arrays.SecondHalf(b);                              //      R[i] = LSB(64, B)

                    a = Arrays.Xor(a, t);
                }
            }
            // 3) Output the results
            byte[][] c = new byte[n + 1][];
            c[0] = a;                                     //  Set C[0] = A
            for (long i = 1; i <= n; i++)                 //  For i = 1 to n
                c[i] = r[i - 1];                          //     C[i] = R[i]

            return Arrays.Concat(c);
        }

        internal byte[] Unwrap(byte[] encryptedCek)
        {
            // 1) Initialize variables
            byte[][] c = Arrays.Slice(encryptedCek, 8);
            byte[] a = c[0];                           //   Set A = C[0]
            byte[][] r = new byte[c.Length - 1][];

            for (int i = 1; i < c.Length; i++)         //   For i = 1 to n
                r[i - 1] = c[i];                       //       R[i] = C[i]

            long n = r.Length;
            // 2) Calculate intermediate values
            for (long j = 5; j >= 0; j--)                                   // For j = 5 to 0
            {
                for (long i = n - 1; i >= 0; i--)                           //   For i = n to 1
                {
                    long t = n * j + i + 1;

                    a = Arrays.Xor(a, t);
                    byte[] B = AesDec(kek, Arrays.Concat(a, r[i]));     //     B = AES-1(K, (A ^ t) | R[i]) where t = n*j+i
                    a = Arrays.FirstHalf(B);                                  //     A = MSB(64, B)
                    r[i] = Arrays.SecondHalf(B);                              //     R[i] = LSB(64, B)
                }
            }

            // 3) Output the results
            if (!Arrays.ConstantTimeEquals(DefaultIV, a))   // If A is an appropriate initial value 
                throw new InvalidSignatureException("AesKeyWrap integrity check failed.");

            // For i = 1 to n
            return Arrays.Concat(r);                        //    P[i] = R[i]
        }

        private byte[] AesEnc(byte[] sharedKey, byte[] plainText)
        {
            using (Aes aes = new AesManaged())
            {
                aes.Key = sharedKey;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        using (CryptoStream encrypt = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            encrypt.Write(plainText, 0, plainText.Length);
                            encrypt.FlushFinalBlock();

                            return ms.ToArray();
                        }
                    }
                }
            }
        }

        private static byte[] AesDec(byte[] sharedKey, byte[] cipherText)
        {
            using (Aes aes = new AesManaged())
            {
                aes.Key = sharedKey;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(cipherText, 0, cipherText.Length);
                            cs.FlushFinalBlock();

                            return ms.ToArray();
                        }
                    }
                }
            }
        }
    }

    class Arrays
    {
        public static byte[][] Slice(byte[] array, int count)
        {
            int sliceCount = array.Length / count;

            byte[][] result = new byte[sliceCount][];


            for (int i = 0; i < sliceCount; i++)
            {
                byte[] slice = new byte[count];

                Buffer.BlockCopy(array, i * count, slice, 0, count);

                result[i] = slice;
            }

            return result;
        }

        public static byte[] Concat(params byte[][] arrays)
        {
            byte[] result = new byte[arrays.Sum(a => (a == null) ? 0 : a.Length)];
            int offset = 0;

            foreach (byte[] array in arrays)
            {
                if (array == null) continue;

                Buffer.BlockCopy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }

            return result;
        }

        public static byte[] FirstHalf(byte[] arr)
        {
            int halfIndex = arr.Length / 2;

            byte[] result = new byte[halfIndex];

            Buffer.BlockCopy(arr, 0, result, 0, halfIndex);

            return result;
        }

        public static byte[] SecondHalf(byte[] arr)
        {
            int halfIndex = arr.Length / 2;

            byte[] result = new byte[halfIndex];

            Buffer.BlockCopy(arr, halfIndex, result, 0, halfIndex);

            return result;
        }

        public static byte[] Xor(byte[] left, long right)
        {
            long _left = BytesToLong(left);
            return LongToBytes(_left ^ right);
        }

        public static long BytesToLong(byte[] array)
        {
            long msb = BitConverter.IsLittleEndian
                        ? (long)(array[0] << 24 | array[1] << 16 | array[2] << 8 | array[3]) << 32
                        : (long)(array[7] << 24 | array[6] << 16 | array[5] << 8 | array[4]) << 32; ;

            long lsb = BitConverter.IsLittleEndian
                           ? (array[4] << 24 | array[5] << 16 | array[6] << 8 | array[7]) & 0x00000000ffffffff
                           : (array[3] << 24 | array[2] << 16 | array[1] << 8 | array[0]) & 0x00000000ffffffff;

            return msb | lsb;
        }

        public static byte[] LongToBytes(long value)
        {
            ulong _value = (ulong)value;

            return BitConverter.IsLittleEndian
                ? new[] { (byte)((_value >> 56) & 0xFF), (byte)((_value >> 48) & 0xFF), (byte)((_value >> 40) & 0xFF), (byte)((_value >> 32) & 0xFF), (byte)((_value >> 24) & 0xFF), (byte)((_value >> 16) & 0xFF), (byte)((_value >> 8) & 0xFF), (byte)(_value & 0xFF) }
                : new[] { (byte)(_value & 0xFF), (byte)((_value >> 8) & 0xFF), (byte)((_value >> 16) & 0xFF), (byte)((_value >> 24) & 0xFF), (byte)((_value >> 32) & 0xFF), (byte)((_value >> 40) & 0xFF), (byte)((_value >> 48) & 0xFF), (byte)((_value >> 56) & 0xFF) };
        }

        public static bool ConstantTimeEquals(byte[] expected, byte[] actual)
        {
            if (expected == actual)
                return true;

            if (expected == null || actual == null)
                return false;

            if (expected.Length != actual.Length)
                return false;

            bool equals = true;

            for (int i = 0; i < expected.Length; i++)
                if (expected[i] != actual[i])
                    equals = false;

            return equals;
        }
    }
}

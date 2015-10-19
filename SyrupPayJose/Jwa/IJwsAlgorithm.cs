namespace SyrupPayJose.Jwa
{
    interface IJwsAlgorithm
    {
        byte[] Sign(byte[] key, byte[] src);
        void Verify(byte[] key, byte[] src, byte[] expected);
    }
}

namespace SyrupPayJose
{
    public interface ISerializeAction
    {
        string Serialization();
        string Deserialization();
        JoseHeader GetHeader();
    }
}

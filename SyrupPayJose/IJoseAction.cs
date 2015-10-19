namespace SyrupPayJose
{
    public interface IJoseAction
    {
        string CompactSerialization();
        string CompactDeserialization();
        JoseHeader GetHeader();
    }
}

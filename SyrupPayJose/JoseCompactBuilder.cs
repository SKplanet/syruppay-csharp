namespace SyrupPayJose
{
    abstract public class JoseCompactBuilder
    {
        protected JoseMethod joseMethod;
        protected JoseSerializeType joseSerializeType;
        protected JoseActionType joseActionType;

        protected string key;

        protected void CompactBuilder(JoseActionType joseActionType)
        {
            CompactBuilder(JoseMethod.NONE, JoseSerializeType.COMPACT_SERIALIZATION, joseActionType);
        }

        protected void CompactBuilder(JoseMethod joseMethod, JoseActionType joseActionType)
        {
            CompactBuilder(joseMethod, JoseSerializeType.COMPACT_SERIALIZATION, joseActionType);
        }

        protected void CompactBuilder(JoseMethod joseMethod, JoseSerializeType joseSerializeType, JoseActionType joseActionType)
        {
            this.joseMethod = joseMethod;
            this.joseSerializeType = joseSerializeType;
            this.joseActionType = joseActionType;
        }

        public JoseActionType GetJoseActionType()
        {
            return joseActionType;
        }

        public JoseCompactBuilder Key(string key)
        {
            this.key = key;
            return this;
        }

        abstract public IJoseAction Create();
    }
}

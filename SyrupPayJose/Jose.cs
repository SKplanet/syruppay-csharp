namespace SyrupPayJose
{
    public class Jose
    {
        public ISerializeAction Configuration(JoseCompactBuilder joseCompactBuilder)
        {
            return new SerializeAction(joseCompactBuilder.Create());
        }

        class SerializeAction : ISerializeAction
        {
            private IJoseAction joseAction;

            public SerializeAction(IJoseAction joseAction)
            {
                this.joseAction = joseAction;
            }

            public string Deserialization()
            {
                return joseAction.CompactDeserialization();
            }

            public string Serialization()
            {
                return joseAction.CompactSerialization();
            }

            public JoseHeader GetHeader()
            {
                return joseAction.GetHeader();
            }
        }
    }
}

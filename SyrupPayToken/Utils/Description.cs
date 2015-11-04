namespace SyrupPayToken.Utils
{
    class Description : System.Attribute
    {
        private string value;

        public Description(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }
}

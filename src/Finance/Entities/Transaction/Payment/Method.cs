namespace Finance.Entities.Transaction.Payment
{
    public class Method
    {
        public Method(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public int Total { get; set; }

        public static implicit operator string(Method method)
        {
            return method.Name;
        }

        public static implicit operator Method(string name)
        {
            return new Method(name);
        }
    }
}
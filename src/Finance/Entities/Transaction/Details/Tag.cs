namespace Finance.Entities.Transaction.Details
{
    public class Tag
    {
        public Tag(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public static implicit operator string(Tag tag)
        {
            return tag.Name;
        }

        public static implicit operator Tag(string name)
        {
            return new Tag(name);
        }
    }
}
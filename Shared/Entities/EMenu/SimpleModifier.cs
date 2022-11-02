namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class SimpleModifier
    {
        public SimpleModifier(Guid id, string name, double minAmount, double maxAmount)
        {
            Id = id;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
    }
}

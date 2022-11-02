namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class DeliveryTerminal
    {
        public DeliveryTerminal() { }

        public DeliveryTerminal(Guid id, string? name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}

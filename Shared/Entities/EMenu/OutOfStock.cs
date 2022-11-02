namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class OutOfStock
    {
        public Guid CorrelationId { get; set; }
        public IEnumerable<TerminalGroupStopList>? TerminalGroupStopLists { get; set; }
    }
}

namespace WebAppAssembly.Shared.Entities.IikoCloudApi
{
    public class AvailableCombo
    {
        /// <summary>
        /// Id of combo specification, describing combo content
        /// </summary>
        public Guid SpecifiecationId { get; set; }
        /// <summary>
        /// Groups contained in combo. If null - there is no suitable product in order yet for that group
        /// </summary>
        public IEnumerable<GroupMapping>? GroupMapping { get; set; }
    }
}

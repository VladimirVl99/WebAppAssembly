using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities.EMenu;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class ModifierItems
    {
        public ModifierItems(IEnumerable<GroupModifier>? groupModifiers = null, IEnumerable<Modifier>? modifiers = null)
        {
            GroupModifiers = groupModifiers;
            Modifiers = modifiers;
        }

        [JsonProperty("groupModifiers")]
        [JsonPropertyName("groupModifiers")]
        public IEnumerable<GroupModifier>? GroupModifiers { get; set; }
        [JsonProperty("modifiers")]
        [JsonPropertyName("modifiers")]
        public IEnumerable<Modifier>? Modifiers { get; set; }
    }
}

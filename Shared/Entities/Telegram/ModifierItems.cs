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

        public IEnumerable<GroupModifier>? GroupModifiers { get; set; }
        public IEnumerable<Modifier>? Modifiers { get; set; }
    }
}

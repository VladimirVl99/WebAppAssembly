using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class PopupButton
    {
        public PopupButton(string id, string text, PopupButtonType type)
        {
            Id = id;
            Type = type;
            Text = text;
        }

        public string Id { get; set; }
        public PopupButtonType Type { get; set; }
        public string Text { get; set; }
    }
}

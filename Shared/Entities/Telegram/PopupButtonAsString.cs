using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class PopupButtonAsString
    {
        public PopupButtonAsString(string id, string text, string type)
        {
            Id = id;
            Type = type;
            Text = text;
        }

        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}

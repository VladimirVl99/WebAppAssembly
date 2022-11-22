using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class PopupParamsAsString
    {
        public PopupParamsAsString(string title, string message, IEnumerable<PopupButtonAsString> buttons)
        {
            Title = title;
            Message = message;
            Buttons = buttons;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public IEnumerable<PopupButtonAsString> Buttons { get; set; }
    }
}

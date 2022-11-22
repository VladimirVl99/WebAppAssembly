using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class PopupParams
    {
        public PopupParams(string title, string message, IEnumerable<PopupButton>? buttons = null)
        {
            Title = title;
            Message = message;
            if (buttons is null)
            {
                Buttons = new List<PopupButton>
                {
                    new PopupButton(string.Empty, "Default popup", PopupButtonType._default)
                };
            }
            else Buttons = buttons;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public IEnumerable<PopupButton> Buttons { get; set; }
    }
}

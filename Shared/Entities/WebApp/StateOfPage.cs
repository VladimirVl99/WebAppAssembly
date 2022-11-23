using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class StateOfPage
    {
        public PageTypeOfOrder PageTypeOfOrder { get; set; }
        public bool RenderComplete { get; set; }
        public bool HaveErrors { get; set; }
        public bool IsPageBlocked { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Models.Order
{
    public class CurrentProduct
    {
        public CurrentProduct() { }
        public CurrentProduct(Guid currentProductId)
        {
            CurrentProductId = currentProductId;
        }
        public CurrentProduct(Guid currentProductId, Guid currentPositionId)
        {
            CurrentProductId = currentProductId;
            CurrentPostionId = currentPositionId;
        }

        public Guid? CurrentProductId { get; set; }
        public Guid? CurrentPostionId { get; set; }
    }
}

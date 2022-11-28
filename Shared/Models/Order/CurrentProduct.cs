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
            ProductId = currentProductId;
        }
        public CurrentProduct(Guid currentProductId, Guid currentPositionId)
        {
            ProductId = currentProductId;
            PostionId = currentPositionId;
        }

        public Guid? ProductId { get; set; }
        public Guid? PostionId { get; set; }
    }
}

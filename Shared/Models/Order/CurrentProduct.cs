using ApiServerForTelegram.Entities.EExceptions;
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public Guid GetProductId() => ProductId ?? throw new InfoException(typeof(CurrentProduct).FullName!, nameof(GetProductId),
            nameof(Exception), $"{typeof(CurrentProduct).FullName!}.{nameof(ProductId)}", ExceptionType.Null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public Guid GetPositionId() => PostionId ?? throw new InfoException(typeof(CurrentProduct).FullName!, nameof(GetPositionId),
            nameof(Exception), $"{typeof(CurrentProduct).FullName!}.{nameof(PostionId)}", ExceptionType.Null);
    }
}

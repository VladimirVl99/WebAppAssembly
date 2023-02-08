using ApiServerForTelegram.Entities.EExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Models.OrderData
{
    public class CurrItem
    {
        public CurrItem() { }
        public CurrItem(Guid currentProductId)
        {
            ProductId = currentProductId;
        }
        public CurrItem(Guid currentProductId, Guid currentPositionId)
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
        public Guid GetProductId() => ProductId ?? throw new InfoException(typeof(CurrItem).FullName!, nameof(GetProductId),
            nameof(Exception), $"{typeof(CurrItem).FullName!}.{nameof(ProductId)}", ExceptionType.Null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public Guid GetPositionId() => PostionId ?? throw new InfoException(typeof(CurrItem).FullName!, nameof(GetPositionId),
            nameof(Exception), $"{typeof(CurrItem).FullName!}.{nameof(PostionId)}", ExceptionType.Null);
    }
}

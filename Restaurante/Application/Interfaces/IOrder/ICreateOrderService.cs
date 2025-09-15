using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder
{
    public interface ICreateOrderService
    {
        Task<long> CreateOrderAsync(OrderRequest request);
    }
}

using Application.Models.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder
{
    public interface IUpdateOrderStatusService
    {
        Task UpdateOrderStatusBasedOnItemsAsync(Order order);
    }
}

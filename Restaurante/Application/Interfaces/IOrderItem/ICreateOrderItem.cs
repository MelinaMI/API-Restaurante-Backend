using Application.Models.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrderItem
{
    public interface ICreateOrderItem
    {
        Task<(List<OrderItem> Items, decimal Total)> CreateItemsAsync(List<Items> itemRequests);
    }
}

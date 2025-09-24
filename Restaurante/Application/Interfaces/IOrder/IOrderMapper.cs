using Application.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder
{
    public interface IOrderMapper
    {
        OrderDetailsResponse ToDetailsResponse(Order order);
        OrderCreateResponse ToOrderCreateResponse(Order order);
        OrderUpdateResponse ToOrderUpdateResponse(Order order);
    }
}

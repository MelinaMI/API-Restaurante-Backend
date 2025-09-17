using Application.Interfaces.IOrder;
using Application.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class OrderMapper : IOrderMapper
    {
        public OrderDetailsResponse ToDetailsResponse(Order order)
        {
            return new OrderDetailsResponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = (double)order.Price,
                DeliverTo = order.DeliveryTo,
                Notes = order.Notes,
                CreatedAt = order.CreateDate,
                UpdatedAt = order.UpdateDate,

                Status = new GenericResponse
                {
                    Id = order.OverallStatusNavigation.Id,
                    Name = order.OverallStatusNavigation.Name
                },

                DeliveryType = new GenericResponse
                {
                    Id = order.DeliveryTypeNavigation.Id,
                    Name = order.DeliveryTypeNavigation.Name
                },

                Items = order.OrderItems.Select(oi => new OrderItemResponse
                {
                    Id = oi.OrderItemId,
                    Quantity = oi.Quantity,
                    Notes = oi.Notes,
                    Status = new GenericResponse
                    {
                        Id = oi.StatusNavigation.Id,
                        Name = oi.StatusNavigation.Name
                    },
                    Dish = new DishShortResponse
                    {
                        Id = oi.DishNavigation.DishId,
                        Name = oi.DishNavigation.Name,
                        Image = oi.DishNavigation.ImageUrl
                    }
                }).ToList()
            };
        }
    }
}

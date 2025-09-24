using Application.Interfaces.IOrder;
using Application.Models.Response;
using Domain.Entities;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Mapper
{
    public class OrderMapper : IOrderMapper
    {
        // Crear orden
        public OrderCreateResponse ToOrderCreateResponse(Order order)
        {
            return new OrderCreateResponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = (double)order.Price,
                CreatedAt = order.CreateDate
            };
        }
        // Actualizar orden
        public OrderUpdateResponse ToOrderUpdateResponse(Order order)
        {
            return new OrderUpdateResponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = (double)order.Price,
                UpdateAt = order.UpdateDate
            };
        }
        // Detalles de la orden
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


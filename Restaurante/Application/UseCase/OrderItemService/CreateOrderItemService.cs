using Application.Interfaces.IDish;
using Application.Interfaces.IOrderItem;
using Application.Models.Request;
using Application.Validators;
using Domain.Entities;
using static Application.Validators.Exceptions;

namespace Application.UseCase.OrderItemService
{
    public class CreateOrderItemService : ICreateOrderItemService
    {
        private readonly IDishQuery _dishQuery;
        public CreateOrderItemService(IDishQuery dishQuery)
        {
            _dishQuery = dishQuery;
        }
        public async Task<(List<OrderItem> Items, decimal Total)> CreateItemsAsync(List<Items> itemRequests)
        {
            decimal total = 0;
            var items = new List<OrderItem>();

            foreach (var itemRequest in itemRequests)
            {
                var dish = await _dishQuery.GetDishByIdAsync(itemRequest.Id);
                if (dish == null || !dish.Available)
                    throw new BadRequestException($"El plato {itemRequest.Id} no está disponible.");

                decimal itemTotal = dish.Price * itemRequest.Quantity;
                total += itemTotal;
                
                var orderItem = new OrderItem
                {
                    Dish = dish.DishId, // FK hacia Dish
                    DishNavigation = dish,
                    Quantity = itemRequest.Quantity,
                    Notes = itemRequest.Notes,
                    Status = 1, // Pending
                    CreateDate = DateTime.UtcNow
                };
                items.Add(orderItem);
            }
            return (items, total);
        }
    }
}

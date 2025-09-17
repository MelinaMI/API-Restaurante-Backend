using Application.Interfaces.IDish;
using Application.Interfaces.IOrderItem;
using Application.Models.Request;
using Domain.Entities;

namespace Application.Services.OrderItemService
{
    public class CreateOrderItemService : ICreateOrderItem
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

                decimal itemTotal = dish.Price * itemRequest.Quantity;
                total += itemTotal;

                items.Add(new OrderItem
                {
                    Dish = dish.DishId,
                    Quantity = itemRequest.Quantity,
                    Notes = itemRequest.Notes,
                    Status = 1,
                    CreateDate = DateTime.UtcNow
                });
            }
            return (items, total);
        }
    }
}

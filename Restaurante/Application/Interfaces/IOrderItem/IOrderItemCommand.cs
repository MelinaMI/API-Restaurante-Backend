using Domain.Entities;

namespace Application.Interfaces.IOrderItem
{
    public interface IOrderItemCommand
    {
        Task<long> InsertOrderItemAsync(OrderItem item);
    }
}

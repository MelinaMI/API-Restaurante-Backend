using Domain.Entities;

namespace Application.Interfaces.IOrder
{
    public interface IOrderQuery
    {
        Task<Order?> GetOrderByIdAsync(long orderId);
        IQueryable<Order> GetAllOrders();
    }
}

using Application.Models.Response;
using Domain.Entities;

namespace Application.Interfaces.IOrder
{
    public interface IGetAllOrders
    {
        Task<IReadOnlyList<OrderDetailsResponse>> GetAllOrdersAsync(DateTime? from, DateTime? to, int? status);
    }
}

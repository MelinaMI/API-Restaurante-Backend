using Application.Models.Response;
using Domain.Entities;

namespace Application.Interfaces.IOrder
{
    public interface IGetAllOrdersService   
    {
        Task<IReadOnlyList<OrderDetailsResponse>> GetAllOrdersAsync(DateTime? from, DateTime? to, int? status);
    }
}

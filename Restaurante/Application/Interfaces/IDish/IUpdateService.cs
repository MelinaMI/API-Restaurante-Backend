using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interfaces.IDish
{
    public interface IUpdateService
    {
        Task<DishResponse> UpdateDishAsync(Guid id, DishUpdateRequest request);
    }
}

using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interfaces.IDish
{
    public interface ICreateService
    {
        Task<DishResponse> CreateDishAsync(DishRequest request);
    }
}

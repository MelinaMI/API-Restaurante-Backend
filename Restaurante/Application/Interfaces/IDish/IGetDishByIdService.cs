using Application.Models.Response;

namespace Application.Interfaces.IDish
{
    public interface IGetDishByIdService
    {
        Task<DishResponse> GetDishByIdAsync(Guid id);
    }
}

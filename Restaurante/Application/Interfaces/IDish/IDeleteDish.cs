using Application.Models.Response;
namespace Application.Interfaces.IDish
{
    public interface IDeleteDish
    {
        Task<DishResponse> DeleteDishAsync(Guid id);
    }
}

using Domain.Entities;

namespace Application.Interfaces.IDish
{
    public interface IDeleteDishValidation
    {
        Task DeleteDishValidationAsync(Dish dish);
    }
}

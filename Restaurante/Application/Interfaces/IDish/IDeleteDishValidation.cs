using Domain.Entities;

namespace Application.Interfaces.IDish
{
    public interface IDeleteDishValidation
    {
        Task DeleteDishValidation(Guid dishId);
    }
}

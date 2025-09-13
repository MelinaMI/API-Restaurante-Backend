using Application.Enum;

namespace Application.Interfaces.IDish
{
    public interface IGetAllDishValidation
    {
        Task ValidateAllAsync(string? name, int? category, OrderPrice? sortByPrice);
    }
}

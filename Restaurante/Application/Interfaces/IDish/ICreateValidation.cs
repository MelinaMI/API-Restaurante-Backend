using Application.Models.Request;

namespace Application.Interfaces.IDish
{
    public interface ICreateValidation
    {
        Task ValidateCreateAsync(DishRequest request);
    }
}

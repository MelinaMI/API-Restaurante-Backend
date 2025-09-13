using Application.Models.Request;

namespace Application.Interfaces.IDish
{
    public interface IUpdateValidation
    {
        Task ValidateUpdateAsync(Guid id, DishUpdateRequest request);
    }
}

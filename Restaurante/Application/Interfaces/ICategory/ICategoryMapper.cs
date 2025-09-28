using Application.Models.Response;
using Domain.Entities;

namespace Application.Interfaces.ICategory
{
    public interface ICategoryMapper
    {
        GenericResponse ToCategoryResponse(Category category);
    }
}

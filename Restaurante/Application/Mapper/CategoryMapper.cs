using Application.Interfaces.ICategory;
using Application.Models.Response;
using Domain.Entities;

namespace Application.Mapper
{
    public class CategoryMapper : ICategoryMapper
    {
        public GenericResponse ToCategoryResponse(Category category)
        {
            return new GenericResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }


}

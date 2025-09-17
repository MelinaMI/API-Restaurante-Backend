using Application.Interfaces.IDish;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;

namespace Application.Mapper
{
    public class DishMapper : IDishMapper
    {
        // Para mapear una lista de platos
        public DishResponse ToDishResponseList(Dish dish, Category category)
        {
            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse
                {
                    Id = category.Id,
                    Name = category.Name
                },
                Image = dish.ImageUrl,
                IsActive = dish.Available,
                CreateAt = dish.CreateDate,
                UpdateAt = dish.UpdateDate
            };
        }
        // Para mapear un solo plato
        public DishResponse ToDishResponse(Dish dish)
        {
            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse
                {
                    Id = dish.CategoryNavigation.Id,
                    Name = dish.CategoryNavigation.Name,
                },
                IsActive = dish.Available,
                CreateAt = dish.CreateDate,
                UpdateAt = dish.UpdateDate,

            };
        }
    }
}

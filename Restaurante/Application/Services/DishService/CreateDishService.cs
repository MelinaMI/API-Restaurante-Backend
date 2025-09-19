using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Mapper;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;

namespace Application.Services.DishService
{
    public class CreateDishService : ICreateService
    {
        private readonly IDishCommand _dishCommand;
        private readonly ICategoryQuery _categoryQuery;
        private readonly IDishMapper _dishMapper;
        private readonly ICreateValidation _createValidator;

        public CreateDishService(IDishCommand dishCommand, ICategoryQuery categoryQuery, IDishMapper dishMapper, ICreateValidation createValidator)
        {
            _dishCommand = dishCommand;
            _categoryQuery = categoryQuery;
            _dishMapper = dishMapper;
            _createValidator = createValidator;
        }

        public async Task<DishResponse> CreateDishAsync(DishRequest request)
        {
            await _createValidator.ValidateCreateAsync(request);

            var dish = new Dish
            {
                DishId = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                Available = true,
                ImageUrl = request.Image,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _dishCommand.InsertDishAsync(dish);

            var category = await _categoryQuery.GetByCategoryIdAsync(dish.Category);
            return _dishMapper.ToDishResponseList(dish, category);
        }
    }
}

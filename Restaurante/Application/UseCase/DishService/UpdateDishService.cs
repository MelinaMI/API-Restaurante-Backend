using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Models.Request;
using Application.Models.Response;

namespace Application.UseCase.Services.DishService
{
    public class UpdateDishService : IUpdateService
    {
        private readonly IDishCommand _dishCommand;
        private readonly ICategoryQuery _categoryQuery;
        private readonly IDishQuery _dishQuery;
        private readonly IDishMapper _dishMapper;
        private readonly IUpdateValidation _updateValidator;

        public UpdateDishService(IDishCommand dishCommand,ICategoryQuery categoryQuery,IDishQuery dishQuery,IDishMapper dishMapper,IUpdateValidation updateValidator)
        {
            _dishCommand = dishCommand;
            _categoryQuery = categoryQuery;
            _dishQuery = dishQuery;
            _dishMapper = dishMapper;
            _updateValidator = updateValidator;
        }

        public async Task<DishResponse> UpdateDishAsync(Guid id, DishUpdateRequest request)
        {
            await _updateValidator.ValidateUpdateAsync(id, request);
            var dish = await _dishQuery.GetDishByIdAsync(id);
            _dishMapper.ToDishUpdate(dish, request);
            await _dishCommand.UpdateDishAsync(dish);
            var category = await _categoryQuery.GetByCategoryIdAsync(dish.Category);
            return _dishMapper.ToDishResponseList(dish, category);
        }
    }
}

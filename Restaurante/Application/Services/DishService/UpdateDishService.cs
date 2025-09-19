using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Models.Request;
using Application.Models.Response;
using static Application.Validators.Exceptions;

namespace Application.Services.DishService
{
    public class UpdateDishService : IUpdateService
    {
        private readonly IDishCommand _dishCommand;
        private readonly ICategoryQuery _categoryQuery;
        private readonly IDishQuery _dishQuery;
        private readonly IDishMapper _dishMapper;
        private readonly IUpdateValidation _updateValidator;
        private readonly IDishMapper _mapper;

        public UpdateDishService(IDishCommand dishCommand,ICategoryQuery categoryQuery,IDishQuery dishQuery,IDishMapper dishMapper,IUpdateValidation updateValidator, IDishMapper mapper)
        {
            _dishCommand = dishCommand;
            _categoryQuery = categoryQuery;
            _dishQuery = dishQuery;
            _dishMapper = dishMapper;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<DishResponse> UpdateDishAsync(Guid id, DishUpdateRequest request)
        {
            // Validar existencia del plato
            var dish = await _dishQuery.GetDishByIdAsync(id);
            if (dish == null)
                throw new NotFoundException("El plato no existe");

            await _updateValidator.ValidateUpdateAsync(id, request);
            // Mapeo de actualización
            _mapper.ToDishUpdate(dish, request);

            await _dishCommand.UpdateDishAsync(dish);

            var category = await _categoryQuery.GetByCategoryIdAsync(dish.Category);

            return _dishMapper.ToDishResponseList(dish, category);
        }
    }
}

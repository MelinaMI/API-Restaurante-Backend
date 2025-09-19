using Application.Interfaces.IDish;
using Application.Models.Response;
using static Application.Validators.Exceptions;

namespace Application.Services.DishService
{
    public class GetDishByIdService : IGetDishByIdService
    {
        private readonly IDishQuery _dishQuery;
        private readonly IDishMapper _mapper;
        private readonly IGetDishByIdValidation _getDishByIdValidator;

        public GetDishByIdService(IDishQuery dishQuery, IDishMapper mapper, IGetDishByIdValidation getDishByIdValidator)
        {
            _dishQuery = dishQuery;
            _mapper = mapper;
            _getDishByIdValidator = getDishByIdValidator;
        }

        public async Task<DishResponse> GetDishByIdAsync(Guid id)
        {
            await _getDishByIdValidator.ValidateByIdAsync(id);

            var dish = await _dishQuery.GetDishByIdAsync(id);

            if (dish == null)
                throw new NotFoundException("El plato no fue encontrado");
            return _mapper.ToDishResponse(dish);
        }
    }
}

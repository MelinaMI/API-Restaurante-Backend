using Application.Interfaces.IDish;
using Application.Models.Response;
using static Application.Validators.Exceptions;

namespace Application.Services.DishService
{
    public class GetDishByIdService : IGetDishByIdService
    {
        private readonly IDishQuery _dishQuery;
        private readonly IDishMapper _mapper;

        public GetDishByIdService(IDishQuery dishQuery, IDishMapper mapper)
        {
            _dishQuery = dishQuery;
            _mapper = mapper;
        }

        public async Task<DishResponse> GetDishByIdAsync(Guid id)
        {
            var dish = await _dishQuery.GetDishByIdAsync(id);
            if (dish == null)
                throw new NotFoundException("El plato no fue encontrado");
            return _mapper.ToDishResponse(dish);
        }
    }
}

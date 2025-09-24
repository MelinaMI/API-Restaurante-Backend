using Application.Enum;
using Application.Interfaces.IDish;
using Application.Models.Response;
using Application.Validators;

namespace Application.UseCase.Services.DishService
{
    public class GetAllDishService : IGetAllDishService
    {
        private readonly IDishQuery _dishQuery;
        private readonly IDishMapper _dishMapper;
        private readonly IGetAllDishValidation _dishValidator;

        public GetAllDishService(IDishQuery dishQuery, IDishMapper dishMapper,IGetAllDishValidation getAllDishValidation)
        {
            _dishQuery = dishQuery;
            _dishMapper = dishMapper;
            _dishValidator = getAllDishValidation;
        }
        public async Task<IReadOnlyList<DishResponse>> GetAllDishesAsync(string? name, int? category, OrderPrice? sortByPrice, bool onlyActive)
        {
            
            await _dishValidator.ValidateAllAsync(name, category, sortByPrice);

            var dishes = await _dishQuery.GetAll();

            if (!string.IsNullOrWhiteSpace(name))
            {
                var normalized = name.Trim().ToLowerInvariant();
                dishes = dishes.Where(d => !string.IsNullOrWhiteSpace(d.Name) && d.Name.ToLowerInvariant().Contains(normalized)).ToList();
            }

            // Filtro por categoría
            if (category.HasValue)
                dishes = dishes.Where(d => d.Category == category.Value).ToList();

            // Filtro por estado activo
            if (onlyActive)
                dishes = dishes.Where(d => d.Available).ToList();

            if (sortByPrice.HasValue)
            {
                dishes = sortByPrice.Value switch
                {
                    OrderPrice.asc => dishes.OrderBy(d => d.Price).ToList(),
                    OrderPrice.desc => dishes.OrderByDescending(d => d.Price).ToList(),
                    _ => dishes
                };
            }
            if (sortByPrice.HasValue && sortByPrice != OrderPrice.asc && sortByPrice != OrderPrice.desc)
            {
                throw new Exceptions.BadRequestException("Parámetros de ordenamiento inválidos");
            }
            return dishes.Select(dish => _dishMapper.ToDishResponse(dish)).ToList();
        }
    }
}

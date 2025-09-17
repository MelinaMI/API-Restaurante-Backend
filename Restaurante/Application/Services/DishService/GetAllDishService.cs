using Application.Enum;
using Application.Interfaces.IDish;
using Application.Models.Response;
using Application.Validators;
using Domain.Entities;
using System;

namespace Application.Services.DishService
{
    public class GetAllDishService : IGetAllDishService
    {
        private readonly IDishQuery _dishQuery;
        private readonly IDishMapper _dishMapper;
        private readonly IGetAllDishValidation _dishValidator;

        public GetAllDishService(IDishQuery dishQuery, IGetDishByIdValidation dishValidator, IDishMapper dishMapper,IGetAllDishValidation getAllDishValidation)
        {
            _dishQuery = dishQuery;
            _dishMapper = dishMapper;
            _dishValidator = getAllDishValidation;
        }
        // Valida los parámetros de entrada
        public async Task<IReadOnlyList<DishResponse>> GetAllDishesAsync(string? name, int? category, OrderPrice? sortByPrice, bool onlyActive)
        {
            // Validación de parámetros
            var dishes = await _dishQuery.GetAllAsync();

            //Filtro por nombre
            if (!string.IsNullOrWhiteSpace(name))
            {
                var normalized = name.Trim().Normalize().ToLowerInvariant();
                dishes = dishes.Where(d => !string.IsNullOrWhiteSpace(d.Name) && d.Name.Normalize().ToLowerInvariant().Contains(normalized)).ToList();
            }

            // Filtro por categoría
            if (category.HasValue)
                dishes = dishes.Where(d => d.Category == category.Value).ToList();

            // Filtro por estado activo
            if (onlyActive)
                dishes = dishes.Where(d => d.Available).ToList();

            _dishValidator.ValidateAllAsync(name, category, sortByPrice);

            if (sortByPrice.HasValue)
            {
                dishes = sortByPrice.Value switch
                {
                    OrderPrice.asc => dishes.OrderBy(d => d.Price).ToList(),
                    OrderPrice.desc => dishes.OrderByDescending(d => d.Price).ToList(),
                    _ => dishes
                };
            }
            return dishes.Select(dish => _dishMapper.ToDishResponse(dish)).ToList();
        }

        // Valida los resultados filtrados
        public void ValidateResults(IReadOnlyList<Dish> dishes, string? name, int? category)
        {
            if (!dishes.Any())
            {
                if (!string.IsNullOrWhiteSpace(name) && category != null)
                    throw new Exceptions.NotFoundException($"No se encontró ningún plato llamado '{name}' en la categoría '{category}'");

                if (!string.IsNullOrWhiteSpace(name))
                    throw new Exceptions.NotFoundException($"No se encontró ningún plato llamado '{name}'");

                if (category != null)
                    throw new Exceptions.NotFoundException($"No se encontraron platos en la categoría '{category}'");

                throw new Exceptions.NotFoundException("No se encontraron platos con los filtros aplicados");
            }
        }

    }
}

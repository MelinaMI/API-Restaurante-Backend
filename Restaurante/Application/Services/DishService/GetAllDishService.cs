using Application.Enum;
using Application.Interfaces.IDish;
using Application.Models.Response;
using Application.Validators;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using static Application.Validators.Exceptions;

namespace Application.Services.DishService
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
        // Valida los parámetros de entrada
        public async Task<IReadOnlyList<DishResponse>> GetAllDishesAsync(string? name, int? category, string? sortByPrice, bool onlyActive)
        {
            // Parsear sortByPrice
            OrderPrice? orderPrice = null;
            if (!string.IsNullOrWhiteSpace(sortByPrice))
            {
                if (!System.Enum.TryParse<OrderPrice>(sortByPrice, ignoreCase: true, out var parsed))
                    throw new BadRequestException("Orden de precio inválido");

                orderPrice = parsed;
            }

            await _dishValidator.ValidateAllAsync(name, category, orderPrice);

            var query = _dishQuery.GetAll();

            //Filtro por nombre
            if (!string.IsNullOrWhiteSpace(name))
            {
                var normalized = name.Trim().ToLowerInvariant();
                query = query.Where(d => d.Name != null && d.Name.ToLower().Contains(normalized));
            }

            // Filtro por categoría
            if (category.HasValue)
                query = query.Where(d => d.Category == category.Value);

            // Filtro por estado activo
            if (onlyActive)
                query = query.Where(d => d.Available);

            // Si tiene valor válido, aplicamos el orden
            if (orderPrice.HasValue)
            {
                query = orderPrice.Value switch
                {
                    OrderPrice.asc => query.OrderBy(d => d.Price),
                    OrderPrice.desc => query.OrderByDescending(d => d.Price),
                    _ => query
                };
            }

            var dishes = await query.ToListAsync(); // Ejecuta el SQL
            return dishes.Select(dish => _dishMapper.ToDishResponse(dish)).ToList();
        }

        /*// Valida los resultados filtrados
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
        }*/

    }
}

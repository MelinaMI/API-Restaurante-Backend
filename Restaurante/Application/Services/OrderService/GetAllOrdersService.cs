using Application.Enum;
using Application.Interfaces.IOrder;
using Application.Models.Response;
using Microsoft.EntityFrameworkCore;
using static Application.Validators.Exceptions;

namespace Application.Services.OrderService
{
    public class GetAllOrdersService : IGetAllOrders
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IOrderMapper _orderMapper;
        private IGetAllOrdersValidation _orderValidator;


        public GetAllOrdersService(IOrderQuery orderQuery, IOrderMapper orderMapper, IGetAllOrdersValidation ordersValidation) 
        { 
            _orderQuery = orderQuery;
            _orderMapper = orderMapper;
            _orderValidator = ordersValidation;
        }
        public async Task<IReadOnlyList<OrderDetailsResponse>> GetAllOrdersAsync(DateTime? from, DateTime? to, int? status)
        {
            await _orderValidator.ValidateGetAllOrder(from, to, status);

            var query = _orderQuery.GetAllOrders(); // IQueryable<Order>

            // Filtros

            // Validación de rango de fechas
            if (from.HasValue && to.HasValue && from > to)
                throw new BadRequestException("La fecha de inicio no puede ser posterior a la fecha de fin.");

            if (from.HasValue)
                query = query.Where(o => o.CreateDate >= from.Value);

            if (to.HasValue)
                query = query.Where(o => o.CreateDate <= to.Value);

            if (status.HasValue)
                query = query.Where(o => o.OverallStatus == status.Value);

            var orders = await query.ToListAsync(); // acá ejecuta el SQL

            return orders.Select(_orderMapper.ToDetailsResponse).ToList();
        }

    }
}

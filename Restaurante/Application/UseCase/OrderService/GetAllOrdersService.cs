using Application.Interfaces.IOrder;
using Application.Models.Response;
using Microsoft.EntityFrameworkCore;
using static Application.Validators.Exceptions;

namespace Application.UseCase.OrderService
{
    public class GetAllOrdersService : IGetAllOrdersService
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

            var query = _orderQuery.GetAllOrders();

            if (status.HasValue)
                query = query.Where(o => o.OverallStatusNavigation.Id == status.Value);

            if (from.HasValue)
                query = query.Where(o => o.CreateDate >= from.Value);

            if (to.HasValue)
                query = query.Where(o => o.CreateDate <= to.Value);

            var orders = await query.ToListAsync();
            return orders.Select(_orderMapper.ToDetailsResponse).ToList();
        }
    }
}

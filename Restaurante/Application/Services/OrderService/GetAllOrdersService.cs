using Application.Interfaces.IOrder;
using Application.Mapper;
using Application.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.OrderService
{
    public class GetAllOrdersService : IGetAllOrders
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IOrderMapper _orderMapper;
        public GetAllOrdersService(IOrderQuery orderQuery, IOrderMapper orderMapper) 
        { 
            _orderQuery = orderQuery;
            _orderMapper = orderMapper;
        }
        public async Task<IReadOnlyList<OrderDetailsResponse>> GetAllOrdersAsync(DateTime? from, DateTime? to, int? status)
        {
            var orders = await _orderQuery.GetAllOrders();

            // Filtros

            // Validación de rango de fechas
            if (from.HasValue && to.HasValue && from > to)
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");

            if (from.HasValue)
                orders = orders.Where(o => o.CreateDate >= from.Value).ToList();

            if (to.HasValue)
                orders = orders.Where(o => o.CreateDate <= to.Value).ToList();

            if (status.HasValue)
                orders = orders.Where(o => o.OverallStatus== status.Value).ToList();

            var response = orders.Select(_orderMapper.ToDetailsResponse).ToList();

            return response;
        }
    }
}

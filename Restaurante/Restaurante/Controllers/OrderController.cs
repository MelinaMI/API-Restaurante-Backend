using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
using Application.Models.Request;
using Application.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICreateOrderService _createOrderService;
        private readonly IGetAllOrdersService _getAllOrdersService;
        private readonly IGetOrderByIdService _getOrderByIdService;
        private readonly IUpdateOrderService _updateOrderService;
        private readonly IUpdateOrderItemStatusService _updateOrderItemStatus;

        public OrderController(ICreateOrderService createOrderService, IGetAllOrdersService getAllOrdersService, IUpdateOrderService updateOrderService, IGetOrderByIdService getOrderByIdService, IUpdateOrderItemStatusService updateOrderItemStatus)
        {
            _createOrderService = createOrderService;
            _getAllOrdersService = getAllOrdersService;
            _getOrderByIdService = getOrderByIdService;
            _updateOrderService = updateOrderService;
            _updateOrderItemStatus = updateOrderItemStatus;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderCreateResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<OrderCreateResponse>> CreateOrder([FromBody] OrderRequest request)
        {
            var response = await _createOrderService.CreateOrderAsync(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<OrderDetailsResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<OrderDetailsResponse>>> GetAllOrders([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? status)
        {
            var orders = await _getAllOrdersService.GetAllOrdersAsync(from, to, status);
            return Ok(orders);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDetailsResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDetailsResponse>> GetOrderById([FromRoute] long id)
        {
            var order = await _getOrderByIdService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrderUpdateResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOrder(long id, [FromBody] OrderUpdateRequest request)
        {
            var updatedOrder = await _updateOrderService.UpdateOrderAsync(id, request);
            return Ok(updatedOrder);
        }
        [HttpPut("{orderId}/item/{itemId}")]
        [ProducesResponseType(typeof(OrderUpdateResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOrderItemStatus([FromRoute] long orderId, [FromRoute] long itemId, [FromBody] OrderItemUpdateRequest request)
        {
            var response = await _updateOrderItemStatus.UpdateOrderItemStatusAsync(orderId, itemId, request.Status);
            return Ok(response);
        }
    }
}

using Application.Interfaces.IOrder;
using Application.Models.Request;
using Application.Models.Response;
using Application.Services.OrderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Application.Validators.Exceptions;

namespace Restaurante.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICreateOrderService _createOrderService;
        private readonly IGetAllOrders _getAllOrdersService;
        private readonly IUpdateOrderService _updateOrderService;
        private readonly IGetOrderById _getOrderByIdService;

        public OrderController(ICreateOrderService createOrderService, IGetAllOrders getAllOrdersService, IUpdateOrderService updateOrderService, IGetOrderById getOrderByIdService)
        {
            _createOrderService = createOrderService;
            _getAllOrdersService = getAllOrdersService;
            _updateOrderService = updateOrderService;
            _getOrderByIdService = getOrderByIdService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderCreateResponse>> CreateOrder([FromBody] OrderRequest request)
        {
            try
            {
                var response = await _createOrderService.CreateOrderAsync(request);
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<OrderDetailsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<OrderDetailsResponse>>> GetAllOrders([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? status)
        {
            try
            {
                var orders = await _getAllOrdersService.GetAllOrdersAsync(from, to, status);
                return Ok(orders);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al obtener las órdenes.", detail = ex.Message });
            }
        }
      
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDetailsResponse>> GetOrderById([FromRoute] long id)
        {
            try
            {
                var order = await _getOrderByIdService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = "Orden no encontrada." });
                }
                return Ok(order);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al obtener la orden.", detail = ex.Message });
            }
        }

      

        /*[HttpPut]
        [ProducesResponseType(typeof(OrderUpdateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrder([FromBody] long id, [FromBody] OrderUpdateRequest request)
        {
            try
            {
                var updatedOrder = await _updateOrderService.UpdateOrder(id, request);
                return Ok(updatedOrder);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }*/

    }
}

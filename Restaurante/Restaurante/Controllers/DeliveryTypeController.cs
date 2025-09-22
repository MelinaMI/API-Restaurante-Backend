using Application.Interfaces.IDeliveryType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeliveryTypeController : ControllerBase
    {
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;
        public DeliveryTypeController(IDeliveryTypeQuery deliveryTypeQuery)
        {
            _deliveryTypeQuery = deliveryTypeQuery;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Domain.Entities.DeliveryType>>> GetAllDeliveryTypes()
        {
            var deliveryTypes = await _deliveryTypeQuery.GetAllDeliveryTypesAsync();
            return Ok(deliveryTypes);
        }
    }
}

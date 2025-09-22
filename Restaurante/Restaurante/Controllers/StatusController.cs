using Application.Interfaces.IStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusQuery _statusQuery;
        public StatusController(IStatusQuery statusQuery)
        {
            _statusQuery = statusQuery;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Domain.Entities.Status>>> GetAllStatuses()
        {
            var statuses = await _statusQuery.GetAllStatusesAsync();
            return Ok(statuses);
        }
    }
}

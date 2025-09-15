using Application.Interfaces.IStatus;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.StatusService
{
    public class GetAllStatusesService : IGetAllStatuses
    {
        private readonly IStatusQuery _statusQuery;
        public GetAllStatusesService(IStatusQuery statusQuery)
        {
            _statusQuery = statusQuery;
        }
        public async Task<IReadOnlyList<Status>> GetAllStatusesAsync()
        {
            return await _statusQuery.GetAllStatusesAsync();
        }
    }
}

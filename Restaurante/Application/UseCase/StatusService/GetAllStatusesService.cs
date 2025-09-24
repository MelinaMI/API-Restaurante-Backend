using Application.Interfaces.IStatus;
using Domain.Entities;

namespace Application.UseCase.StatusService
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

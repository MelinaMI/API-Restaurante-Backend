using Domain.Entities;

namespace Application.Interfaces.IStatus
{
    public interface IStatusQuery
    {
        Task<string> GetStatusByIdAsync(int id);
        Task<int> GetStatusByNameAsync(string name);
        Task<IReadOnlyList<Status>> GetAllStatusesAsync();

    }
}

using Application.Interfaces.IStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class StatusQuery : IStatusQuery
    {
        public Task<IReadOnlyList<string>> GetAllStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetStatusByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetStatusByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}

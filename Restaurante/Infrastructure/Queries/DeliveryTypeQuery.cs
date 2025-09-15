using Application.Interfaces.IDeliveryType;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;

namespace Infrastructure.Queries
{
    public class DeliveryTypeQuery : IDeliveryTypeQuery
    {
        private readonly AppDbContext _context;
        public DeliveryTypeQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<DeliveryType>> GetAllDeliveryTypesAsync()
        {
            return await _context.DeliveryTypes.AsNoTracking().ToListAsync();
        }

        public Task<DeliveryType> GetDeliveryTypeByIdAsync(int DeliveryId)
        {
            throw new NotImplementedException();
        }

        public Task<DeliveryType> GetDeliveryTypeByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}

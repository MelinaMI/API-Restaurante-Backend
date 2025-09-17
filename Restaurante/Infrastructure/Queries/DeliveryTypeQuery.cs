using Application.Interfaces.IDeliveryType;
using Application.Models.Response;
using Application.Validators;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

        public async Task<DeliveryType> GetDeliveryTypeByIdAsync(int DeliveryId)
        {
            {
                var deliveryType = await _context.DeliveryTypes
                    .FirstOrDefaultAsync(dt => dt.Id == DeliveryId);

                if (deliveryType == null)
                    throw new Exceptions.BadRequestException($"Tipo de entrega no encontrado: {DeliveryId}");

                return deliveryType;
            }
        }

        public Task<DeliveryType> GetDeliveryTypeByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}

using Application.Interfaces.IDeliveryType;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class DeliveryTypeQuery : IDeliveryTypeQuery
    {
        public Task<IReadOnlyList<DeliveryType>> GetAllDeliveryTypesAsync()
        {
            throw new NotImplementedException();
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

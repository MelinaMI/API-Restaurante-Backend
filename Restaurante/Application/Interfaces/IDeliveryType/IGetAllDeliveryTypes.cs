using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDeliveryType
{
    public interface IGetAllDeliveryTypes
    {
        Task<IReadOnlyList<DeliveryType>> GetAllDeliveryTypesAsync();
    }
}

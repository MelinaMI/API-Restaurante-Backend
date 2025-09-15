using Application.Interfaces.ICategory;
using Application.Interfaces.IDeliveryType;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DeliveryTypeService
{
    public class GetAllDeliveryTypesService : IGetAllDeliveryTypes
    {
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;
        public GetAllDeliveryTypesService(IDeliveryTypeQuery deliveryTypeQuery)
        {
            _deliveryTypeQuery = deliveryTypeQuery;
        }
        public async Task<IReadOnlyList<DeliveryType>> GetAllDeliveryTypesAsync()
        {
            return await _deliveryTypeQuery.GetAllDeliveryTypesAsync();

            //return (IReadOnlyList<DeliveryType>)categ.Select(c => c.Name).ToList();
        }
    }
}

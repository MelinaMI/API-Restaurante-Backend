using Application.Interfaces.IDeliveryType;
using Domain.Entities;

namespace Application.UseCase.DeliveryTypeService
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
        }
    }
}

using Domain.Entities;

namespace Application.Interfaces.IDeliveryType
{
    public interface IDeliveryTypeQuery
    {
        Task<DeliveryType> GetDeliveryTypeByIdAsync(int DeliveryId);
        Task<IReadOnlyList<DeliveryType>> GetAllDeliveryTypesAsync();
    }
}

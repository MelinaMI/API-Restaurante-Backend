namespace Application.Interfaces.IOrder
{
    public interface IGetOrderByIdValidation
    {
        Task  ValidateOrderById(long orderId);
    }
}

namespace Application.Interfaces.IDish
{
    public interface IGetDishByIdValidation
    {
        Task ValidateByIdAsync(Guid id);
    }
}

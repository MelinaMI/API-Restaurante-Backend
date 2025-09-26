using Application.Interfaces.IStatus;
using Application.Models.Response;
using Domain.Entities;

namespace Application.Mapper
{
    public class StatusMapper : IStatusMapper
    {
        public GenericResponse ToGenericResponse(Status status)
        {
            return new GenericResponse
            {
                Id = status.Id,
                Name = status.Name
            };
        }
    }
}

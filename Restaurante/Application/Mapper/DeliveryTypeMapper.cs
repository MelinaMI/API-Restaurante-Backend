using Application.Interfaces.IDeliveryType;
using Application.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class DeliveryTypeMapper : IDeliveryTypeMapper
    {
        public GenericResponse ToGenericResponse(DeliveryType delivery)
        {
            return new GenericResponse
            {
                Id = delivery.Id,
                Name = delivery.Name
            };
        }
    }
}

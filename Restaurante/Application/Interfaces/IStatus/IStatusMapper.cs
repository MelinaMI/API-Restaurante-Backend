using Application.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IStatus
{
    public interface IStatusMapper
    {
        GenericResponse ToGenericResponse(Status status);
    }
}

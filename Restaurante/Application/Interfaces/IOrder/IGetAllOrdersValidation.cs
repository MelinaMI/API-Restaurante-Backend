using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder
{
    public interface IGetAllOrdersValidation
    {
        Task ValidateGetAllOrder(DateTime? from, DateTime? to, int? status);
    }
}

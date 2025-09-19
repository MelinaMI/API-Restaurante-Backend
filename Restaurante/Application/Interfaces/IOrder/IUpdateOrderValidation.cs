using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder
{
    public interface IUpdateOrderValidation
    {
        Task UpdateOrderValidation(long id, List<Items> items);
    }
}

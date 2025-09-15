using Application.Interfaces.IOrder;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.OrderValidator
{
    public class CreateOrderValidator : ICreateOrderValidation
    {
        public Task ValidateOrderAsync(OrderRequest order)
        {
            throw new NotImplementedException();
        }
    }
}

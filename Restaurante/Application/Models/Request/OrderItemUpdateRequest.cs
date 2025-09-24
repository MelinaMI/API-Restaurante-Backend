using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class OrderItemUpdateRequest
    {
        public required int Status { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class OrderItemUpdateRequest
    {
        [Required]
        public int Status { get; set; }
    }
}

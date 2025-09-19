using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class Items
    {
        public required Guid Id { get; set; }
        
        public required int Quantity { get; set; }
        public string? Notes { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Application.Models.Response
{
    public class Delivery
    {
        [Required]
        public int Id { get; set; }
        public string To { get; set; }
    }
}

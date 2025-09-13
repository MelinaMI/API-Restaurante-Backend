namespace Application.Models.Request
{
    public class DishUpdateRequest
    {
        public required string Name { get; set; }
        public string Description { get; set; }
        public required decimal Price { get; set; }
        public required int Category { get; set; }
        public string Image { get; set; }
        public required bool IsActive { get; set; }
    }
}

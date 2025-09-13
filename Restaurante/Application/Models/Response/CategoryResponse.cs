namespace Application.Models.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; } //PK
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}

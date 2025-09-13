namespace Application.Models.Response
{
    public class OrderDetailsResponse
    {
        public long OrderNumber { get; set; }
        public double TotalAmount { get; set; }
        public string DeliverTo { get; set; }
        public string Notes { get; set; }
        public GenericResponse Status { get; set; }
        public GenericResponse Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

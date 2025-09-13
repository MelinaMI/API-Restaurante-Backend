namespace Application.Models.Response
{
    public class OrderCreateResponse
    {
        public long OrderNumber { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

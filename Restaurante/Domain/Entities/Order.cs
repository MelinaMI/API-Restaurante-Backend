using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public long OrderId { get; set; } //PK
        public string DeliveryTo { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        // Relacion 1:N con OrderItem - una orden tiene muchos items
        public ICollection<OrderItem> OrderItems { get; set; }

        // FK hacia Status - estado general de la orden
        public int OverallStatus { get; set; } //FK
        public Status OverallStatusNavigation { get; set; }

        // FK hacia DeliveryType - un tipo de entrega tiene muchas ordenes
        public int DeliveryType { get; set; } //FK
        public DeliveryType DeliveryTypeNavigation { get; set; }
    }
}

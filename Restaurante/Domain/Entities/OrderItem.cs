using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public DateTime CreateDate { get; set; }

        // FK hacia Dish - un OrderItem referencia a UN plato específico 
        public Guid Dish { get; set; }
        public Dish DishNavigation { get; set; }

        //FK hacia Order - un OrderItem 
        public long Order { get; set; }
        public Order OrderNavigation { get; set; }

        // FK hacia Status - un OrderItem tiene UN estado
        public int Status { get; set; }
        public Status StatusNavigation { get; set; }
    }
}

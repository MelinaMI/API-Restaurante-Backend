using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; set; } //PK
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        // Relación 1:N Una categoria puede tener muchos platos
        public ICollection<Dish> Dishes { get; set; }
    }
}

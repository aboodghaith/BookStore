using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class OrderItem : BaseModel
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}

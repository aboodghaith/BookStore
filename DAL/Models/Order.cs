using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Order : BaseModel
    {

        public string UserId { get; set; }

        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ICollection<OrderItem>? OrderItems { get; set; }

        public Payment? Payment { get; set; }

    }
}

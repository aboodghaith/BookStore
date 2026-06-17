using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Payment : BaseModel
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod Method { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}

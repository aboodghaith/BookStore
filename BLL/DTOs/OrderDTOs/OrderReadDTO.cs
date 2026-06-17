using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.OrderDTOs
{
    public class OrderReadDTO
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

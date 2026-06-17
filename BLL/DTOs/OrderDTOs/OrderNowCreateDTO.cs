using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.OrderDTOs
{
    public class OrderNowCreateDTO
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public PaymentMethod Method { get; set; } = PaymentMethod.Cash;
    }
}

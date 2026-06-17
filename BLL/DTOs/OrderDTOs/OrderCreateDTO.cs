using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.OrderDTOs
{
    public class OrderCreateDTO
    {
        public PaymentMethod Method { get; set; } = PaymentMethod.Cash;
    }
}

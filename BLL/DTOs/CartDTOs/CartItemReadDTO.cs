using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CartDTOs
{
    public class CartItemReadDTO
    {
        public int CartItemId { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public decimal PricePerUnit { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => PricePerUnit * Quantity;
    }
}

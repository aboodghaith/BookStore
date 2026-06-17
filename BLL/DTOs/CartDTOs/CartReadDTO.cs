using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CartDTOs
{
    public class CartReadDTO
    {

        public int CartId { get; set; }
        public IList<CartItemReadDTO> CartItems { get; set; }

        public decimal TotalCartPrice { get; set; }
    }
}

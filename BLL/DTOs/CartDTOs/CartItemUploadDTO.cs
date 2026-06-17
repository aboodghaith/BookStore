using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CartDTOs
{
    public class CartItemUploadDTO
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}

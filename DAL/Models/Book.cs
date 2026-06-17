using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Book : BaseModel
    {

        public string Title { get; set; }

        public string Author { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string Description { get; set; }

        public string CoverImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }


        public ICollection<CartItem>? CartItems { get; set; }
    }
}

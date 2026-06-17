using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Cart : BaseModel
    {
        public string UserId { get; set; } 

        public User? User { get; set; }

        public ICollection<CartItem>? CartItems { get; set; }
        


    }
}

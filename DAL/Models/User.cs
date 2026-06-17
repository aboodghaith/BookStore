using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? ImagePath { get; set; }

        public bool IsDeleted { get; set; } = false;

        
        public string Address { get; set; }


        public ICollection<Order>? Orders { get; set; }

        public Cart? Cart { get; set; }
    }
}

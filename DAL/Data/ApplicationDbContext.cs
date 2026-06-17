using DAL.Configruations;
using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new OrderConfig());
            builder.ApplyConfiguration(new OrderItemConfig());
            builder.ApplyConfiguration(new BookConfig());

            builder.Entity<Payment>().HasOne(p => p.Order).WithOne(o => o.Payment).HasForeignKey<Payment>(p => p.OrderId);

            builder.Entity<Payment>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);


            base.OnModelCreating(builder);
        }
    }
}

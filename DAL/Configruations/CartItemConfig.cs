using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configruations
{
    public class CartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasIndex(ci => ci.CartId);
            builder.HasIndex(ci => ci.BookId);


            builder.HasOne(ci => ci.Cart).WithMany(c => c.CartItems).HasForeignKey(ci => ci.CartId). OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(ci => ci.Book).WithMany(b => b.CartItems).HasForeignKey(ci => ci.BookId).OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(b => !b.IsDeleted);


        }
    }
}

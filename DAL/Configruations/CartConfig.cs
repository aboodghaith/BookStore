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
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasIndex(c => c.UserId);

            builder.HasOne(c => c .User).WithOne(u => u.Cart).HasForeignKey<Cart>(x => x.UserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(b => !b.IsDeleted);

        }
    }
}

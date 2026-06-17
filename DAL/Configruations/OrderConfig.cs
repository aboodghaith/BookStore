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
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(o => o.UserId);
            builder.HasIndex(o => o.Status);
            builder.HasIndex(o => new { o.UserId, o.Status });
            builder.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
            builder.HasQueryFilter(o => !o.IsDeleted);

           
        }
    }
}

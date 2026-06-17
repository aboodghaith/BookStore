using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Configruations
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(oi => oi.Order)
             .WithMany(o => o.OrderItems)
             .HasForeignKey(o => o.OrderId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(oi => oi.Book)
                   .WithMany()
                   .HasForeignKey(oi => oi.BookId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(oi => oi.Price).HasColumnType("decimal(18,2)");

            builder.HasIndex(oi => oi.OrderId);
            builder.HasIndex(oi => oi.BookId);

            builder.HasQueryFilter(oi => !oi.IsDeleted);

        }
    }
}

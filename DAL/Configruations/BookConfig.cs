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
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Description).HasMaxLength(500);
            builder.HasOne(b => b.Category).WithMany(c => c.Books).HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(b => b.CategoryId);
            builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Price).HasColumnType("decimal(18,2)");

            builder.HasQueryFilter(b => !b.IsDeleted);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Book_Stock_NonNegative", "[StockQuantity] >= 0");
            });
        }
    }
}

using Ecom.core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(N => N.Name).IsRequired().HasMaxLength(50);
            builder.Property(D => D.Description).IsRequired();
            builder.Property(P => P.NewPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.OldPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(I => I.ID).IsRequired();
            builder.HasData(
                new Product{ID=1,Name="test", Description="test", CategoryID=1, NewPrice=500 }
            );

        }
    }
}

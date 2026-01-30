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
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(N => N.Name).IsRequired().HasMaxLength(50);
            builder.Property(I=>I.ID).IsRequired();
            builder.HasData(
                new Category {ID=1,Name="test", Description="test" }
                );
        }
    }
}

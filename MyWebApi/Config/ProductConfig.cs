using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Config
{
    public class ProductConfig : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("T_Products");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(50).IsRequired().IsUnicode();
            builder.Property(t => t.Price).HasColumnType("decimal(8,2)");
            builder.Property(t => t.Description).HasMaxLength(1024).IsUnicode();
        }
    }
}

using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(i => i.SaleId).IsRequired().HasColumnType("uuid");
        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.UnitPrice).IsRequired().HasColumnType("decimal(18, 2)");
        builder.Property(i => i.Discount).IsRequired().HasColumnType("decimal(5, 2)");
        builder.Property(i => i.TotalPrice).IsRequired().HasColumnType("decimal(18, 2)");
        builder.Property(i => i.IsCancelled).IsRequired().HasDefaultValue(false);

        builder.OwnsOne(i => i.Product, productBuilder =>
        {
            productBuilder.Property(p => p.ExternalId).HasColumnName("ProductId").IsRequired().HasColumnType("uuid");
            productBuilder.Property(p => p.Name).HasColumnName("ProductName").IsRequired().HasMaxLength(200);
            productBuilder.Property(p => p.Price).HasColumnName("ProductBasePrice").IsRequired().HasColumnType("decimal(18, 2)");
        });
    }
}

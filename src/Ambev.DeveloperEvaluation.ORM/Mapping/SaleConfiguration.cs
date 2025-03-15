using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(50);
        builder.Property(s => s.SaleDate).IsRequired().HasColumnType("timestamp");
        builder.Property(s => s.TotalAmount).IsRequired().HasColumnType("decimal(18, 2)");
        builder.Property(s => s.IsCancelled).IsRequired().HasDefaultValue(false);

        builder.OwnsOne(s => s.Customer, customerBuilder =>
        {
            customerBuilder.Property(c => c.ExternalId).HasColumnName("CustomerId").IsRequired().HasColumnType("uuid");
            customerBuilder.Property(c => c.Name).HasColumnName("CustomerName").IsRequired().HasMaxLength(200);
        });

        builder.OwnsOne(s => s.Branch, branchBuilder =>
        {
            branchBuilder.Property(b => b.ExternalId).HasColumnName("BranchId").IsRequired().HasColumnType("uuid");
            branchBuilder.Property(b => b.Name).HasColumnName("BranchName").IsRequired().HasMaxLength(200);
        });

        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(i => i.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<DateTime>("CreatedAt").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property<DateTime?>("UpdatedAt");
    }
}

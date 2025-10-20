using BankSoftware.Domain.Constants.Enums;
using BankSoftware.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSoftware.Infrastructure.Data.Configurations
{
    internal class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder
                .ToTable("loans")
                .HasKey(k => k.Id);

            builder.Property(k => k.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Number).HasMaxLength(50).IsRequired();

            builder
                .Property(p => p.Status)
                .HasConversion(value => value.ToString(), db => (LoanStatus)Enum.Parse(typeof(LoanStatus), db))
                .HasColumnName("Status")
                .IsRequired();

            builder.HasIndex(i => i.Number).IsUnique();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniBanking.Domain.Entities;

namespace MiniBanking.Infrastructure.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasIndex(u => u.Reference).IsUnique();
        builder.Ignore(x => x.TransactionStatus);
        builder.Ignore(x => x.TransactionType);
        builder.Property(x => x.Reference).HasMaxLength(50);
    }
}
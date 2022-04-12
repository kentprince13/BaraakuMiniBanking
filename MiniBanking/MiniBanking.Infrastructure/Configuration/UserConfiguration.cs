using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniBanking.Domain.Entities;

namespace MiniBanking.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(c => c.FirstName).IsRequired();
        builder.HasIndex(c => c.Email).IsUnique();
        builder.Property(c => c.PhoneNumber).IsRequired();
        builder.Property(p => p.Image).HasColumnType("text");
        builder.Property(p => p.IsActive).HasDefaultValue(false);
        builder.HasMany(x => x.Transactions).WithOne(c => c.InitiatedByUser)
            .HasForeignKey(x => x.InitiatedBy).OnDelete(DeleteBehavior.Restrict);

    }
}
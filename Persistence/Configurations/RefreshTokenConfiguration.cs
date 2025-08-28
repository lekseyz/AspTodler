using Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();
        
        builder.HasOne(t => t.User)
            .WithMany(u => u.RefreshTokens).
            HasForeignKey(t => t.UserId);
        
        builder.HasOne(t => t.NextToken)
            .WithOne(t => t.PreviousToken)
            .HasForeignKey<RefreshTokenEntity>(t => t.NextTokenId);
    }
}
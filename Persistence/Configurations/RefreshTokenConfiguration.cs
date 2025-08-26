using Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens).
            HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.NextToken)
            .WithOne(t => t.PreviousToken)
            .HasForeignKey<RefreshTokenEntity>(t => t.NextTokenId);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.DbEntities;

namespace Persistence.Configurations;

public class NoteInfoConfiguration : IEntityTypeConfiguration<NoteInfoEntity>
{
    public void Configure(EntityTypeBuilder<NoteInfoEntity> builder)
    {
        builder.HasKey(n => n.Id);

        builder.HasOne(n => n.Creator)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.CreatorId);
    }
}
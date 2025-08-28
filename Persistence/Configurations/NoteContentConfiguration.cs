using Domain.ValueTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.DbEntities;

namespace Persistence.Configurations;

public class NoteContentConfiguration : IEntityTypeConfiguration<NoteContentEntity>
{
    public void Configure(EntityTypeBuilder<NoteContentEntity> builder)
    {
        builder.HasIndex(n => n.Id);
    }
}
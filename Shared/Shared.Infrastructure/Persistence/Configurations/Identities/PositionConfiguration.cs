using Identity.Domain.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("position");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(x => x.ModifiedAt)
                .HasColumnName("modified_at")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ModifiedBy)
                .HasColumnName("modified_by");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
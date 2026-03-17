using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.Domain.Groups;

namespace Identity.Infrastructure.Persistence.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("group");

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
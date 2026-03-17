using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.Domain.Departments;

namespace Identity.Infrastructure.Persistence.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("department");

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

            // 🌳 Tree
            builder.Property(x => x.Index)
                .HasColumnName("index")
                .HasDefaultValue(1);

            builder.Property(x => x.Level)
                .HasColumnName("level")
                .HasDefaultValue(1);

            builder.Property(x => x.PathCode)
                .HasColumnName("path_code")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.ParentId)
                .HasColumnName("parent_id");

            // 🌳 Self reference
            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Childrens)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 📌 Index
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.ParentId);
            builder.HasIndex(x => x.PathCode);
        }
    }
}
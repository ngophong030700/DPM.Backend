using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workflow.Domain.WorkflowCategories;

namespace Shared.Infrastructure.Persistence.Configurations.Workflows
{
    public class WorkflowCategoryConfiguration : IEntityTypeConfiguration<WorkflowCategory>
    {
        public void Configure(EntityTypeBuilder<WorkflowCategory> builder)
        {
            builder.ToTable("workflow_category", "workflow");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.HasKey(u => u.Id);

            builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
            builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(1000);
            builder.Property(e => e.Icon).HasColumnName("icon").HasMaxLength(100);

            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.CreatedBy).HasColumnName("created_by");
            builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            builder.HasIndex(u => u.IsDeleted);
        }
    }
}
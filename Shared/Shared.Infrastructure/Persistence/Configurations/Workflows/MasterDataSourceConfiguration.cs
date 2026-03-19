using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workflow.Domain.MasterDataSources;

namespace Shared.Infrastructure.Persistence.Configurations.Workflows
{
    public class MasterDataSourceConfiguration : IEntityTypeConfiguration<MasterDataSource>
    {
        public void Configure(EntityTypeBuilder<MasterDataSource> builder)
        {
            builder.ToTable("master_data_source", "workflow");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.HasKey(u => u.Id);

            builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
            builder.Property(e => e.Code).HasColumnName("code").IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(1000);
            builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);

            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.CreatedBy).HasColumnName("created_by");
            builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            builder.HasIndex(u => u.Code).IsUnique();
            builder.HasIndex(u => u.IsDeleted);

            builder.HasMany(e => e.Columns)
                .WithOne()
                .HasForeignKey(c => c.SourceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Values)
                .WithOne()
                .HasForeignKey(v => v.SourceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MasterDataColumnConfiguration : IEntityTypeConfiguration<MasterDataColumn>
    {
        public void Configure(EntityTypeBuilder<MasterDataColumn> builder)
        {
            builder.ToTable("master_data_column", "workflow");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.HasKey(u => u.Id);

            builder.Property(e => e.SourceId).HasColumnName("source_id");
            builder.Property(e => e.ColumnKey).HasColumnName("column_key").IsRequired().HasMaxLength(100);
            builder.Property(e => e.ColumnLabel).HasColumnName("column_label").IsRequired().HasMaxLength(255);
            builder.Property(e => e.DataType).HasColumnName("data_type").IsRequired().HasMaxLength(50);
            builder.Property(e => e.IsRequired).HasColumnName("is_required");
            builder.Property(e => e.SortOrder).HasColumnName("sort_order").HasDefaultValue(0);

            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.CreatedBy).HasColumnName("created_by");
            builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            builder.HasIndex(u => u.IsDeleted);
        }
    }

    public class MasterDataValueConfiguration : IEntityTypeConfiguration<MasterDataValue>
    {
        public void Configure(EntityTypeBuilder<MasterDataValue> builder)
        {
            builder.ToTable("master_data_value", "workflow");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.HasKey(u => u.Id);

            builder.Property(e => e.SourceId).HasColumnName("source_id");
            builder.Property(e => e.DisplayName).HasColumnName("display_name").IsRequired().HasMaxLength(255);
            builder.Property(e => e.ValueCode).HasColumnName("value_code").IsRequired().HasMaxLength(100);
            builder.Property(e => e.SortOrder).HasColumnName("sort_order").HasDefaultValue(0);
            builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);

            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.CreatedBy).HasColumnName("created_by");
            builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            builder.HasIndex(u => u.IsDeleted);

            builder.HasMany(e => e.Cells)
                .WithOne()
                .HasForeignKey(c => c.ValueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MasterDataCellConfiguration : IEntityTypeConfiguration<MasterDataCell>
    {
        public void Configure(EntityTypeBuilder<MasterDataCell> builder)
        {
            builder.ToTable("master_data_cell", "workflow");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.HasKey(u => u.Id);

            builder.Property(e => e.ValueId).HasColumnName("value_id");
            builder.Property(e => e.ColumnId).HasColumnName("column_id");
            builder.Property(e => e.CellValue).HasColumnName("cell_value");

            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.CreatedBy).HasColumnName("created_by");
            builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        }
    }
}
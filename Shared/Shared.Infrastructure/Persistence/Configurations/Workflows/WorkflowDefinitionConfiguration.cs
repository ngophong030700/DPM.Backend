using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workflow.Domain.WorkflowDefinitions;

namespace Shared.Infrastructure.Persistence.Configurations.Workflows;

public class WorkflowDefinitionConfiguration : IEntityTypeConfiguration<WorkflowDefinition>
{
    public void Configure(EntityTypeBuilder<WorkflowDefinition> builder)
    {
        builder.ToTable("workflow_definition", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Code).HasColumnName("code").IsRequired().HasMaxLength(100);
        builder.Property(e => e.CategoryId).HasColumnName("category_id");
        builder.Property(e => e.Icon).HasColumnName("icon").HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(1000);
        builder.Property(e => e.Permissions).HasColumnName("permissions");

        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

        builder.HasMany(e => e.Versions)
               .WithOne()
               .HasForeignKey(v => v.WorkflowId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.Code).IsUnique().HasFilter("[is_deleted] = 0");
        builder.HasIndex(e => e.IsDeleted);
    }
}

public class WorkflowVersionConfiguration : IEntityTypeConfiguration<WorkflowVersion>
{
    public void Configure(EntityTypeBuilder<WorkflowVersion> builder)
    {
        builder.ToTable("workflow_version", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.WorkflowId).HasColumnName("workflow_id");
        builder.Property(e => e.VersionName).HasColumnName("version_name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.IsActive).HasColumnName("is_active");
        builder.Property(e => e.Status).HasColumnName("status");
        builder.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(1000);

        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
    }
}

public class WorkflowStepDefineConfiguration : IEntityTypeConfiguration<WorkflowStepDefine>
{
    public void Configure(EntityTypeBuilder<WorkflowStepDefine> builder)
    {
        builder.ToTable("workflow_step_define", "workflow");

        // ReactFlow string ID
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);

        builder.Property(e => e.VersionId).HasColumnName("version_id");
        builder.Property(e => e.Label).HasColumnName("label").IsRequired().HasMaxLength(255);
        builder.Property(e => e.StepType).HasColumnName("step_type");
        builder.Property(e => e.StatusCode).HasColumnName("status_code").HasMaxLength(100);
        builder.Property(e => e.AssignRule).HasColumnName("assign_rule");
        builder.Property(e => e.AssignValueJson).HasColumnName("assign_value_json");
        builder.Property(e => e.SlaTime).HasColumnName("sla_time");
        builder.Property(e => e.SlaUnit).HasColumnName("sla_unit");
        builder.Property(e => e.PositionX).HasColumnName("position_x");
        builder.Property(e => e.PositionY).HasColumnName("position_y");
        builder.Property(e => e.IsSignatureStep).HasColumnName("is_signature_step");

        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

        builder.HasMany(e => e.Actions)
               .WithOne()
               .HasForeignKey(a => a.StepId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Documents)
               .WithOne()
               .HasForeignKey(d => d.StepId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.FieldPermissions)
               .WithOne()
               .HasForeignKey(p => p.StepId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Hooks)
               .WithOne()
               .HasForeignKey(h => h.StepId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class WorkflowStepDefineActionConfiguration : IEntityTypeConfiguration<WorkflowStepDefineAction>
{
    public void Configure(EntityTypeBuilder<WorkflowStepDefineAction> builder)
    {
        builder.ToTable("workflow_step_define_action", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.StepId).HasColumnName("step_id").IsRequired().HasMaxLength(50);
        builder.Property(e => e.ButtonKey).HasColumnName("button_key").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Label).HasColumnName("label").IsRequired().HasMaxLength(255);
        builder.Property(e => e.TargetStepId).HasColumnName("target_step_id").HasMaxLength(50);
        builder.Property(e => e.NotifyTemplate).HasColumnName("notify_template");
        builder.Property(e => e.SortOrder).HasColumnName("sort_order");

        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

        builder.HasMany(e => e.Rules)
               .WithOne()
               .HasForeignKey(r => r.ActionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class WorkflowActionRuleConfiguration : IEntityTypeConfiguration<WorkflowActionRule>
{
    public void Configure(EntityTypeBuilder<WorkflowActionRule> builder)
    {
        builder.ToTable("workflow_action_rule", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.ActionId).HasColumnName("action_id");
        builder.Property(e => e.ConditionExpression).HasColumnName("condition_expression");
        builder.Property(e => e.TargetStepId).HasColumnName("target_step_id").IsRequired().HasMaxLength(50);
        builder.Property(e => e.SortOrder).HasColumnName("sort_order");
    }
}

public class WorkflowStepDefineDocumentConfiguration : IEntityTypeConfiguration<WorkflowStepDefineDocument>
{
    public void Configure(EntityTypeBuilder<WorkflowStepDefineDocument> builder)
    {
        builder.ToTable("workflow_step_define_document", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.StepId).HasColumnName("step_id").IsRequired().HasMaxLength(50);
        builder.Property(e => e.DocTypeName).HasColumnName("doc_type_name").IsRequired().HasMaxLength(255);
        builder.Property(e => e.IsRequired).HasColumnName("is_required");
        builder.Property(e => e.CheckDigitalSignature).HasColumnName("check_digital_signature");
        builder.Property(e => e.SortOrder).HasColumnName("sort_order");

        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
    }
}

public class WorkflowStepFieldPermissionConfiguration : IEntityTypeConfiguration<WorkflowStepFieldPermission>
{
    public void Configure(EntityTypeBuilder<WorkflowStepFieldPermission> builder)
    {
        builder.ToTable("workflow_step_field_permission", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.StepId).HasColumnName("step_id").IsRequired().HasMaxLength(50);
        builder.Property(e => e.FieldId).HasColumnName("field_id");
        builder.Property(e => e.Permission).HasColumnName("permission");
        builder.Property(e => e.IsRequired).HasColumnName("is_required");
    }
}

public class WorkflowStepHookConfiguration : IEntityTypeConfiguration<WorkflowStepHook>
{
    public void Configure(EntityTypeBuilder<WorkflowStepHook> builder)
    {
        builder.ToTable("workflow_step_hook", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.StepId).HasColumnName("step_id").IsRequired().HasMaxLength(50);
        builder.Property(e => e.EventType).HasColumnName("event_type");
        builder.Property(e => e.ActionType).HasColumnName("action_type");
        builder.Property(e => e.ConfigJson).HasColumnName("config_json").IsRequired();
        builder.Property(e => e.SortOrder).HasColumnName("sort_order");
    }
}

public class WorkflowFieldConfiguration : IEntityTypeConfiguration<WorkflowField>
{
    public void Configure(EntityTypeBuilder<WorkflowField> builder)
    {
        builder.ToTable("workflow_field", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.VersionId).HasColumnName("version_id");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Label).HasColumnName("label").IsRequired().HasMaxLength(255);
        builder.Property(e => e.DataType).HasColumnName("data_type");
        builder.Property(e => e.DataSourceType).HasColumnName("data_source_type");
        builder.Property(e => e.DataSourceConfigJson).HasColumnName("data_source_config_json");
        builder.Property(e => e.FieldFormula).HasColumnName("field_formula");
        builder.Property(e => e.SettingsJson).HasColumnName("settings_json");
        builder.Property(e => e.SortOrder).HasColumnName("sort_order");
        builder.Property(e => e.IsRequired).HasColumnName("is_required");

        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

        builder.HasMany(e => e.GridColumns)
               .WithOne()
               .HasForeignKey(g => g.ParentFieldId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class WorkflowGridColumnConfiguration : IEntityTypeConfiguration<WorkflowGridColumn>
{
    public void Configure(EntityTypeBuilder<WorkflowGridColumn> builder)
    {
        builder.ToTable("workflow_grid_column", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.ParentFieldId).HasColumnName("parent_field_id");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Label).HasColumnName("label").IsRequired().HasMaxLength(255);
        builder.Property(e => e.DataType).HasColumnName("data_type");
        builder.Property(e => e.DataSourceType).HasColumnName("data_source_type");
        builder.Property(e => e.DataSourceConfigJson).HasColumnName("data_source_config_json");
        builder.Property(e => e.SettingsJson).HasColumnName("settings_json");
        builder.Property(e => e.SortOrder).HasColumnName("sort_order");
        builder.Property(e => e.IsRequired).HasColumnName("is_required");

        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
    }
}

public class WorkflowLayoutConfiguration : IEntityTypeConfiguration<WorkflowLayout>
{
    public void Configure(EntityTypeBuilder<WorkflowLayout> builder)
    {
        builder.ToTable("workflow_layout", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.VersionId).HasColumnName("version_id");
        builder.Property(e => e.RowsJson).HasColumnName("rows_json").IsRequired();
        builder.Property(e => e.AttachmentSettingsJson).HasColumnName("attachment_settings_json");

        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
    }
}

public class WorkflowReportConfiguration : IEntityTypeConfiguration<WorkflowReport>
{
    public void Configure(EntityTypeBuilder<WorkflowReport> builder)
    {
        builder.ToTable("workflow_report", "workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.VersionId).HasColumnName("version_id");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(e => e.FieldsConfigJson).HasColumnName("fields_config_json");
        builder.Property(e => e.ChartConfigJson).HasColumnName("chart_config_json");
        builder.Property(e => e.IsActive).HasColumnName("is_active");

        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
    }
}
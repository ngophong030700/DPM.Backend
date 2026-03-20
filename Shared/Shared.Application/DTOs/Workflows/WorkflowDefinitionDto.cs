using System.ComponentModel.DataAnnotations;
using Workflow.Domain.WorkflowDefinitions;

namespace Shared.Application.DTOs.Workflows
{
    #region ===================== VIEW DTOs =====================

    public class ViewListWorkflowDefinitionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public int CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public int ActiveVersionId { get; set; }
        public string? ActiveVersionName { get; set; }
    }

    public class ViewDetailWorkflowDefinitionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public string? Permissions { get; set; }
        public int CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public int ModifiedById { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public List<ViewWorkflowVersionDto> Versions { get; set; } = new();
    }

    public class ViewWorkflowVersionDto
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string VersionName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public WorkflowVersionStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ViewWorkflowReportDto
    {
        public int Id { get; set; }
        public int VersionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? FieldsConfigJson { get; set; }
        public string? ChartConfigJson { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    #endregion

    #region ===================== METADATA DTOs =====================

    public class WorkflowDefinitionMetadataDto
    {
        public List<MetadataItemDto> Categories { get; set; } = new();
        public List<MetadataItemDto> Users { get; set; } = new();
        public List<MetadataItemDto> Groups { get; set; } = new();
        public List<string> Icons { get; set; } = new();
    }

    public class WorkflowFieldMetadataDto
    {
        public List<MasterDataSourceMetadataDto> MasterDataSources { get; set; } = new();
        public List<MetadataItemDto> FieldDataTypes { get; set; } = new();
    }

    public class WorkflowStepMetadataDto
    {
        public List<MetadataItemDto> NotificationTemplates { get; set; } = new();
        public List<MetadataItemDto> Users { get; set; } = new();
        public List<MetadataItemDto> Groups { get; set; } = new();
        public List<MetadataItemDto> DocumentTypes { get; set; } = new();
        public List<MetadataItemDto> UserFields { get; set; } = new();
    }

    public class MetadataItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
    }

    public class MasterDataSourceMetadataDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public List<string> Columns { get; set; } = new();
    }

    #endregion

    #region ===================== WORKFLOW DEFINITION COMMANDS =====================

    public class CreateWorkflowDefinitionDto
    {
        [Required(ErrorMessage = "Tên quy trình là bắt buộc.")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã quy trình là bắt buộc.")]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        [MaxLength(255)]
        public string? Icon { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        public string? Permissions { get; set; }
    }

    public class UpdateWorkflowDefinitionDto
    {
        [Required(ErrorMessage = "Tên quy trình là bắt buộc.")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã quy trình là bắt buộc.")]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        [MaxLength(255)]
        public string? Icon { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        public string? Permissions { get; set; }
    }

    #endregion

    #region ===================== VERSION COMMANDS =====================

    public class CreateWorkflowVersionDto
    {
        [Required]
        public int WorkflowId { get; set; }

        [Required(ErrorMessage = "Tên phiên bản là bắt buộc.")]
        [MaxLength(100)]
        public string VersionName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }

    public class CloneWorkflowVersionDto
    {
        public string? Notes { get; set; }
    }

    public class ReorderItemDto
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
    }

    public class StepPositionDto
    {
        public double PositionX { get; set; }
        public double PositionY { get; set; }
    }

    #endregion

    #region ===================== CONFIGURATION COMMANDS =====================

    // FIELDS
    public class SetupWorkflowFieldsDto
    {
        public List<FieldConfigDto> Fields { get; set; } = new();
    }

    public class FieldConfigDto
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Label { get; set; } = string.Empty;
        public FieldDataType DataType { get; set; }
        public DataSourceType? DataSourceType { get; set; }
        public string? DataSourceConfigJson { get; set; }
        public string? FieldFormula { get; set; }
        public string? SettingsJson { get; set; }
        public int SortOrder { get; set; }
        public bool IsRequired { get; set; }

        public List<GridColumnConfigDto> GridColumns { get; set; } = new();
    }

    public class GridColumnConfigDto
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Label { get; set; } = string.Empty;
        public FieldDataType DataType { get; set; }
        public DataSourceType? DataSourceType { get; set; }
        public string? DataSourceConfigJson { get; set; }
        public string? SettingsJson { get; set; }
        public int SortOrder { get; set; }
        public bool IsRequired { get; set; }
    }

    // LAYOUT
    public class SetupWorkflowLayoutDto
    {
        [Required]
        public string RowsJson { get; set; } = string.Empty;
        public string? AttachmentSettingsJson { get; set; }
    }

    // STEPS
    public class SetupWorkflowStepsDto
    {
        public List<StepConfigDto> Steps { get; set; } = new();
    }

    public class StepConfigDto
    {
        [Required]
        public string Id { get; set; } = string.Empty; // ReactFlow Node ID
        [Required]
        public string Label { get; set; } = string.Empty;
        public WorkflowStepType StepType { get; set; }
        public string? StatusCode { get; set; }
        public AssignRule AssignRule { get; set; }
        public string? AssignValueJson { get; set; }
        public int? SlaTime { get; set; }
        public SlaUnit? SlaUnit { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public bool IsSignatureStep { get; set; }

        public List<StepActionConfigDto> Actions { get; set; } = new();
        public List<StepDocumentConfigDto> Documents { get; set; } = new();
        public List<StepFieldPermissionConfigDto> FieldPermissions { get; set; } = new();
        public List<StepHookConfigDto> Hooks { get; set; } = new();
    }

    public class StepActionConfigDto
    {
        public int? Id { get; set; }
        [Required]
        public string ButtonKey { get; set; } = string.Empty;
        [Required]
        public string Label { get; set; } = string.Empty;
        public string? TargetStepId { get; set; }
        public string? NotifyTemplate { get; set; }
        public int SortOrder { get; set; }
        
        public List<ActionRuleConfigDto> Rules { get; set; } = new();
    }

    public class ActionRuleConfigDto
    {
        public int? Id { get; set; }
        public string? ConditionExpression { get; set; }
        [Required]
        public string TargetStepId { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

    public class StepDocumentConfigDto
    {
        public int? Id { get; set; }
        [Required]
        public string DocTypeName { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public bool CheckDigitalSignature { get; set; }
        public int SortOrder { get; set; }
    }

    public class StepFieldPermissionConfigDto
    {
        public int? Id { get; set; }
        public int FieldId { get; set; }
        public FieldPermissionType Permission { get; set; }
        public bool IsRequired { get; set; }
    }

    public class StepHookConfigDto
    {
        public int? Id { get; set; }
        public HookEventType EventType { get; set; }
        public HookActionType ActionType { get; set; }
        [Required]
        public string ConfigJson { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

    // REPORTS
    public class SetupWorkflowReportDto
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? FieldsConfigJson { get; set; }
        public string? ChartConfigJson { get; set; }
    }

    #endregion
}
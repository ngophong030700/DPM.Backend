using Shared.Application.DTOs.Workflows;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Mappings
{
    public static class WorkflowDefinitionMapping
    {
        public static ViewListWorkflowDefinitionDto? ToListDto(this WorkflowDefinition entity)
        {
            if (entity == null) return null;

            return new ViewListWorkflowDefinitionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                CategoryId = entity.CategoryId,
                Icon = entity.Icon,
                Description = entity.Description,
                CreatedById = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
                ActiveVersionId = entity.Versions.FirstOrDefault(v => v.IsActive)?.Id ?? 0,
                ActiveVersionName = entity.Versions.FirstOrDefault(v => v.IsActive)?.VersionName
            };
        }

        public static ViewDetailWorkflowDefinitionDto? ToDetailDto(this WorkflowDefinition entity)
        {
            if (entity == null) return null;

            return new ViewDetailWorkflowDefinitionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                CategoryId = entity.CategoryId,
                Icon = entity.Icon,
                Description = entity.Description,
                Permissions = entity.Permissions,
                CreatedById = entity.CreatedBy,
                ModifiedById = entity.ModifiedBy,
                CreatedAt = entity.CreatedAt,
                ModifiedAt = entity.ModifiedAt,
                Versions = entity.Versions.Select(v => v.ToVersionDto()!).ToList()
            };
        }

        public static ViewWorkflowVersionDto? ToVersionDto(this WorkflowVersion entity)
        {
            if (entity == null) return null;

            return new ViewWorkflowVersionDto
            {
                Id = entity.Id,
                WorkflowId = entity.WorkflowId,
                VersionName = entity.VersionName,
                IsActive = entity.IsActive,
                Status = entity.Status,
                Notes = entity.Notes,
                CreatedAt = entity.CreatedAt
            };
        }

        public static FieldConfigDto ToFieldConfigDto(this WorkflowField entity)
        {
            return new FieldConfigDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Label = entity.Label,
                DataType = entity.DataType,
                DataSourceType = entity.DataSourceType,
                DataSourceConfigJson = entity.DataSourceConfigJson,
                FieldFormula = entity.FieldFormula,
                SettingsJson = entity.SettingsJson,
                SortOrder = entity.SortOrder,
                IsRequired = entity.IsRequired,
                GridColumns = entity.GridColumns.Select(c => c.ToGridColumnConfigDto()).ToList()
            };
        }

        public static GridColumnConfigDto ToGridColumnConfigDto(this WorkflowGridColumn entity)
        {
            return new GridColumnConfigDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Label = entity.Label,
                DataType = entity.DataType,
                DataSourceType = entity.DataSourceType,
                DataSourceConfigJson = entity.DataSourceConfigJson,
                SettingsJson = entity.SettingsJson,
                SortOrder = entity.SortOrder,
                IsRequired = entity.IsRequired
            };
        }

        public static StepConfigDto ToStepConfigDto(this WorkflowStepDefine entity)
        {
            return new StepConfigDto
            {
                Id = entity.Id,
                Label = entity.Label,
                StepType = entity.StepType,
                StatusCode = entity.StatusCode,
                AssignRule = entity.AssignRule,
                AssignValueJson = entity.AssignValueJson,
                SlaTime = entity.SlaTime,
                SlaUnit = entity.SlaUnit,
                PositionX = entity.PositionX,
                PositionY = entity.PositionY,
                IsSignatureStep = entity.IsSignatureStep,
                Actions = entity.Actions.Select(a => a.ToActionConfigDto()).ToList(),
                Documents = entity.Documents.Select(d => d.ToDocumentConfigDto()).ToList(),
                FieldPermissions = entity.FieldPermissions.Select(p => p.ToFieldPermissionConfigDto()).ToList(),
                Hooks = entity.Hooks.Select(h => h.ToHookConfigDto()).ToList()
            };
        }

        public static StepActionConfigDto ToActionConfigDto(this WorkflowStepDefineAction entity)
        {
            return new StepActionConfigDto
            {
                Id = entity.Id,
                ButtonKey = entity.ButtonKey,
                Label = entity.Label,
                TargetStepId = entity.TargetStepId,
                NotifyTemplate = entity.NotifyTemplate,
                SortOrder = entity.SortOrder,
                Rules = entity.Rules.Select(r => r.ToActionRuleConfigDto()).ToList()
            };
        }

        public static ActionRuleConfigDto ToActionRuleConfigDto(this WorkflowActionRule entity)
        {
            return new ActionRuleConfigDto
            {
                Id = entity.Id,
                ConditionExpression = entity.ConditionExpression,
                TargetStepId = entity.TargetStepId,
                SortOrder = entity.SortOrder
            };
        }

        public static StepDocumentConfigDto ToDocumentConfigDto(this WorkflowStepDefineDocument entity)
        {
            return new StepDocumentConfigDto
            {
                Id = entity.Id,
                DocTypeName = entity.DocTypeName,
                IsRequired = entity.IsRequired,
                CheckDigitalSignature = entity.CheckDigitalSignature,
                SortOrder = entity.SortOrder
            };
        }

        public static StepFieldPermissionConfigDto ToFieldPermissionConfigDto(this WorkflowStepFieldPermission entity)
        {
            return new StepFieldPermissionConfigDto
            {
                Id = entity.Id,
                FieldId = entity.FieldId,
                Permission = entity.Permission,
                IsRequired = entity.IsRequired
            };
        }

        public static StepHookConfigDto ToHookConfigDto(this WorkflowStepHook entity)
        {
            return new StepHookConfigDto
            {
                Id = entity.Id,
                EventType = entity.EventType,
                ActionType = entity.ActionType,
                ConfigJson = entity.ConfigJson,
                SortOrder = entity.SortOrder
            };
        }

        public static ViewWorkflowReportDto ToReportDto(this WorkflowReport entity)
        {
            return new ViewWorkflowReportDto
            {
                Id = entity.Id,
                VersionId = entity.VersionId,
                Name = entity.Name,
                FieldsConfigJson = entity.FieldsConfigJson,
                ChartConfigJson = entity.ChartConfigJson,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
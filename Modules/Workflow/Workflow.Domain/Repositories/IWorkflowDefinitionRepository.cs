using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Domain.Repositories
{
    public interface IWorkflowDefinitionRepository
    {
        Task<WorkflowDefinition?> GetByIdAsync(int id, bool includeDetails = false);
        Task<WorkflowDefinition?> GetByCodeAsync(string code, bool includeDetails = false);
        Task<WorkflowDefinition> CreateAsync(WorkflowDefinition workflowDefinition);
        Task<WorkflowDefinition> UpdateAsync(WorkflowDefinition workflowDefinition);
        Task<bool> SoftDeleteAsync(int id, int modifiedBy);
        Task<bool> IsCodeExistsAsync(string code, int? excludeId = null);
        
        // Version management
        Task<WorkflowVersion?> GetVersionByIdAsync(int versionId);
        Task<WorkflowVersion> CreateVersionAsync(WorkflowVersion version);
        Task UpdateVersionAsync(WorkflowVersion version);
        Task<bool> SoftDeleteVersionAsync(int versionId, int modifiedBy);

        // Configurations
        Task<List<WorkflowField>> GetFieldsByVersionIdAsync(int versionId);
        Task<WorkflowField?> GetFieldByIdAsync(int id);
        Task SaveFieldsAsync(int versionId, List<WorkflowField> fields);
        Task SaveFieldAsync(WorkflowField field);
        Task<bool> DeleteFieldAsync(int id, int modifiedBy);

        Task<WorkflowGridColumn?> GetGridColumnByIdAsync(int id);
        Task SaveGridColumnAsync(WorkflowGridColumn column);
        Task<bool> DeleteGridColumnAsync(int id, int modifiedBy);

        Task<WorkflowLayout?> GetLayoutByVersionIdAsync(int versionId);
        Task SaveLayoutAsync(WorkflowLayout layout);

        Task<List<WorkflowStepDefine>> GetStepsByVersionIdAsync(int versionId);
        Task<WorkflowStepDefine?> GetStepByIdAsync(string id, int versionId);
        Task SaveStepsAsync(int versionId, List<WorkflowStepDefine> steps);
        Task SaveStepAsync(WorkflowStepDefine step);
        Task<bool> DeleteStepAsync(string id, int versionId, int modifiedBy);

        Task<WorkflowStepDefineAction?> GetActionByIdAsync(int id);
        Task SaveActionAsync(WorkflowStepDefineAction action);
        Task<bool> DeleteActionAsync(int id, int modifiedBy);

        Task<WorkflowStepDefineDocument?> GetDocumentByIdAsync(int id);
        Task SaveDocumentAsync(WorkflowStepDefineDocument document);
        Task<bool> DeleteDocumentAsync(int id, int modifiedBy);

        Task<List<WorkflowReport>> GetReportsByVersionIdAsync(int versionId);
        Task<WorkflowReport?> GetReportByIdAsync(int id);
        Task SaveReportAsync(WorkflowReport report);
        Task<bool> DeleteReportAsync(int reportId, int modifiedBy);
    }
}
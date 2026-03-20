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

        // Configurations
        Task<List<WorkflowField>> GetFieldsByVersionIdAsync(int versionId);
        Task SaveFieldsAsync(int versionId, List<WorkflowField> fields);

        Task<WorkflowLayout?> GetLayoutByVersionIdAsync(int versionId);
        Task SaveLayoutAsync(WorkflowLayout layout);

        Task<List<WorkflowStepDefine>> GetStepsByVersionIdAsync(int versionId);
        Task SaveStepsAsync(int versionId, List<WorkflowStepDefine> steps);

        Task<List<WorkflowReport>> GetReportsByVersionIdAsync(int versionId);
        Task SaveReportAsync(WorkflowReport report);
        Task<bool> DeleteReportAsync(int reportId, int modifiedBy);
    }
}
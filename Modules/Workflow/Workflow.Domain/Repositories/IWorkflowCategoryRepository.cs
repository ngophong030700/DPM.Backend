using Workflow.Domain.WorkflowCategories;

namespace Workflow.Domain.Repositories
{
    public interface IWorkflowCategoryRepository
    {
        Task<WorkflowCategory?> GetByIdAsync(int id);
        Task<WorkflowCategory> CreateAsync(WorkflowCategory workflowCategory);
        Task<WorkflowCategory> UpdateAsync(WorkflowCategory workflowCategory);
        Task<bool> SoftDeleteAsync(int id, int modifiedBy);
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
    }
}
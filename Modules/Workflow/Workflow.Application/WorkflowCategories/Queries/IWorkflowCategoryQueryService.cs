using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowCategories.Queries
{
    public interface IWorkflowCategoryQueryService
    {
        Task<ViewDetailWorkflowCategoryDto?> GetByIdAsync(int id);
        Task<PagingResponse<ViewListWorkflowCategoryDto>> GetListAsync(PagingRequest request);
    }
}
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Queries
{
    public interface IWorkflowDefinitionQueryService
    {
        Task<ViewDetailWorkflowDefinitionDto?> GetByIdAsync(int id);
        Task<PagingResponse<ViewListWorkflowDefinitionDto>> GetListAsync(PagingRequest request);
        Task<ViewWorkflowVersionDto?> GetVersionByIdAsync(int versionId);
        
        // Cấu hình version
        Task<List<FieldConfigDto>> GetFieldsByVersionIdAsync(int versionId);
        Task<SetupWorkflowLayoutDto?> GetLayoutByVersionIdAsync(int versionId);
        Task<List<StepConfigDto>> GetStepsByVersionIdAsync(int versionId);
        Task<List<ViewWorkflowReportDto>> GetReportsByVersionIdAsync(int versionId);

        // Metadata
        Task<WorkflowDefinitionMetadataDto> GetDefinitionMetadataAsync();
        Task<WorkflowFieldMetadataDto> GetFieldMetadataAsync();
        Task<WorkflowStepMetadataDto> GetStepMetadataAsync(int versionId);
    }
}
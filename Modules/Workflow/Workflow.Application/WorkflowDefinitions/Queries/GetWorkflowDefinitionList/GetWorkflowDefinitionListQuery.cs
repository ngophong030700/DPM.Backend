using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Queries.GetWorkflowDefinitionList
{
    public class GetWorkflowDefinitionListQuery : IRequest<PagingResponse<ViewListWorkflowDefinitionDto>>
    {
        public PagingRequest Request { get; set; }

        public GetWorkflowDefinitionListQuery(PagingRequest request)
        {
            Request = request;
        }
    }
}
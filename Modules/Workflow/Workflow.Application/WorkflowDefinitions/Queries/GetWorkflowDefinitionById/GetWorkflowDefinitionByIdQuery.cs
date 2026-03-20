using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Queries.GetWorkflowDefinitionById
{
    public class GetWorkflowDefinitionByIdQuery : IRequest<ViewDetailWorkflowDefinitionDto?>
    {
        public int Id { get; set; }

        public GetWorkflowDefinitionByIdQuery(int id)
        {
            Id = id;
        }
    }
}
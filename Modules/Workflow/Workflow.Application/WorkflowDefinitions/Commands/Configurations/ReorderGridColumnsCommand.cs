using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class ReorderGridColumnsCommand : IRequest<bool>
    {
        public List<ReorderItemDto> Data { get; set; } = new();

        public ReorderGridColumnsCommand(List<ReorderItemDto> data)
        {
            Data = data;
        }
    }
}
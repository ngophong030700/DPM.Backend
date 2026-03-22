using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class ReorderFieldsCommand : IRequest<bool>
    {
        public List<ReorderItemDto> Data { get; set; } = new();

        public ReorderFieldsCommand(List<ReorderItemDto> data)
        {
            Data = data;
        }
    }
}
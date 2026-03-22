using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateFieldCommand : IRequest<FieldConfigDto?>
    {
        public int Id { get; set; }
        public FieldConfigDto Data { get; set; }

        public UpdateFieldCommand(int id, FieldConfigDto data)
        {
            Id = id;
            Data = data;
        }
    }
}
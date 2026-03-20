using MediatR;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteWorkflowReportCommand : IRequest<bool>
    {
        public int ReportId { get; set; }

        public DeleteWorkflowReportCommand(int reportId)
        {
            ReportId = reportId;
        }
    }
}
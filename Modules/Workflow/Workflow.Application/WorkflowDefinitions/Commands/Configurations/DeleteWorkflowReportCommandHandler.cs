using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteWorkflowReportCommandHandler : IRequestHandler<DeleteWorkflowReportCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteWorkflowReportCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteWorkflowReportCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var result = await _repository.DeleteReportAsync(request.ReportId, userId);
            
            if (!result)
            {
                throw new NotFoundException("Không tìm thấy báo cáo.");
            }

            return true;
        }
    }
}
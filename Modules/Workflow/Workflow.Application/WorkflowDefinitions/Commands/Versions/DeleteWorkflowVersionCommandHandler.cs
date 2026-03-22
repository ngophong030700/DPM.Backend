using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class DeleteWorkflowVersionCommandHandler : IRequestHandler<DeleteWorkflowVersionCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteWorkflowVersionCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteWorkflowVersionCommand request, CancellationToken cancellationToken)
        {
            var success = await _repository.SoftDeleteVersionAsync(request.Id, _currentUserService.UserId);
            if (!success)
            {
                throw new NotFoundException("Không tìm thấy phiên bản cần xóa.");
            }
            return true;
        }
    }
}
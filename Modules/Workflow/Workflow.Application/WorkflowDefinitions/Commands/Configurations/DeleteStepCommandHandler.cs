using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteStepCommandHandler : IRequestHandler<DeleteStepCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteStepCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteStepCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var result = await _repository.DeleteStepAsync(request.Id, request.VersionId, userId);
            if (!result)
            {
                throw new NotFoundException("Không tìm thấy bước quy trình cần xóa.");
            }

            return true;
        }
    }
}

using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteActionCommandHandler : IRequestHandler<DeleteActionCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteActionCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteActionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var result = await _repository.DeleteActionAsync(request.Id, userId);
            if (!result)
            {
                throw new NotFoundException("Không tìm thấy hành động bước quy trình cần xóa.");
            }

            return true;
        }
    }
}

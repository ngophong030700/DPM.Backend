using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteFieldCommandHandler : IRequestHandler<DeleteFieldCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteFieldCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteFieldCommand request, CancellationToken cancellationToken)
        {
            var success = await _repository.DeleteFieldAsync(request.Id, _currentUserService.UserId);
            if (!success)
            {
                throw new NotFoundException("Không tìm thấy trường dữ liệu cần xóa.");
            }
            return true;
        }
    }
}
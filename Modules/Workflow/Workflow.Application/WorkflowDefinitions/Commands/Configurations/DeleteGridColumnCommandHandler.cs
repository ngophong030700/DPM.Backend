using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteGridColumnCommandHandler : IRequestHandler<DeleteGridColumnCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteGridColumnCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteGridColumnCommand request, CancellationToken cancellationToken)
        {
            var success = await _repository.DeleteGridColumnAsync(request.Id, _currentUserService.UserId);
            if (!success)
            {
                throw new NotFoundException("Không tìm thấy cột cần xóa.");
            }
            return true;
        }
    }
}
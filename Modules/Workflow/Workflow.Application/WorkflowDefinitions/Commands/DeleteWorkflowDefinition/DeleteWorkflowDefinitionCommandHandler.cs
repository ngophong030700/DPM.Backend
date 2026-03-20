using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.DeleteWorkflowDefinition
{
    public class DeleteWorkflowDefinitionCommandHandler : IRequestHandler<DeleteWorkflowDefinitionCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteWorkflowDefinitionCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteWorkflowDefinitionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            
            var result = await _repository.SoftDeleteAsync(request.Id, userId);
            if (!result)
            {
                throw new NotFoundException("Không tìm thấy quy trình.");
            }

            return true;
        }
    }
}
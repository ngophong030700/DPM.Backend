using MediatR;
using Shared.Application.Common.Interfaces;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowCategories.Commands.DeleteWorkflowCategory
{
    public class DeleteWorkflowCategoryCommandHandler
        : IRequestHandler<DeleteWorkflowCategoryCommand, bool>
    {
        private readonly IWorkflowCategoryRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteWorkflowCategoryCommandHandler(
            IWorkflowCategoryRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            DeleteWorkflowCategoryCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.SoftDeleteAsync(request.Id, _currentUser.UserId);
        }
    }
}
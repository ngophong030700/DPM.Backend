using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class ActivateWorkflowVersionCommandHandler : IRequestHandler<ActivateWorkflowVersionCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public ActivateWorkflowVersionCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(ActivateWorkflowVersionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var workflow = await _repository.GetByIdAsync(request.WorkflowId, includeDetails: true);

            if (workflow == null)
            {
                throw new NotFoundException("Không tìm thấy quy trình.");
            }

            workflow.ActivateVersion(request.VersionId, userId);
            
            await _repository.UpdateAsync(workflow);

            return true;
        }
    }
}
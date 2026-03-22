using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class ActivateWorkflowVersionByIdCommandHandler : IRequestHandler<ActivateWorkflowVersionByIdCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public ActivateWorkflowVersionByIdCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(ActivateWorkflowVersionByIdCommand request, CancellationToken cancellationToken)
        {
            var version = await _repository.GetVersionByIdAsync(request.Id);
            if (version == null)
            {
                throw new NotFoundException("Không tìm thấy phiên bản.");
            }

            var workflow = await _repository.GetByIdAsync(version.WorkflowId, includeDetails: true);
            if (workflow == null)
            {
                throw new NotFoundException("Không tìm thấy quy trình.");
            }

            workflow.ActivateVersion(version.Id, _currentUserService.UserId);
            await _repository.UpdateAsync(workflow);

            return true;
        }
    }
}
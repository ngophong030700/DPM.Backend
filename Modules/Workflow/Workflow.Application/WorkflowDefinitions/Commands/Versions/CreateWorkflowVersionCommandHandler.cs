using MediatR;
using Shared.Application.DTOs.Workflows;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Application.WorkflowDefinitions.Queries;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class CreateWorkflowVersionCommandHandler : IRequestHandler<CreateWorkflowVersionCommand, ViewWorkflowVersionDto?>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly IWorkflowDefinitionQueryService _queryService;
        private readonly ICurrentUserService _currentUserService;

        public CreateWorkflowVersionCommandHandler(
            IWorkflowDefinitionRepository repository,
            IWorkflowDefinitionQueryService queryService,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUserService = currentUserService;
        }

        public async Task<ViewWorkflowVersionDto?> Handle(CreateWorkflowVersionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var workflow = await _repository.GetByIdAsync(request.WorkflowId, includeDetails: true);

            if (workflow == null)
            {
                throw new NotFoundException("Không tìm thấy quy trình.");
            }

            var version = workflow.AddVersion(request.Data.VersionName, request.Data.Notes, userId);
            
            await _repository.UpdateAsync(workflow);

            return await _queryService.GetVersionByIdAsync(version.Id);
        }
    }
}
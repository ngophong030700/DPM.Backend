using Newtonsoft.Json;
using MediatR;
using Shared.Application.DTOs.Workflows;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Application.WorkflowDefinitions.Queries;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.CreateWorkflowDefinition
{
    public class CreateWorkflowDefinitionCommandHandler : IRequestHandler<CreateWorkflowDefinitionCommand, ViewDetailWorkflowDefinitionDto?>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly IWorkflowDefinitionQueryService _queryService;
        private readonly ICurrentUserService _currentUserService;

        public CreateWorkflowDefinitionCommandHandler(
            IWorkflowDefinitionRepository repository,
            IWorkflowDefinitionQueryService queryService,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUserService = currentUserService;
        }

        public async Task<ViewDetailWorkflowDefinitionDto?> Handle(CreateWorkflowDefinitionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var data = request.Data;

            if (await _repository.IsCodeExistsAsync(data.Code))
            {
                throw new DomainException($"Mã quy trình '{data.Code}' đã tồn tại.");
            }

            var permissionsJson = JsonConvert.SerializeObject(new WorkflowPermissionsJsonDto
            {
                CreatePermissions = data.CreatePermissions,
                ViewPermissions = data.ViewPermissions
            });

            var definition = WorkflowDefinition.Create(
                name: data.Name,
                code: data.Code,
                categoryId: data.CategoryId,
                icon: data.Icon,
                description: data.Description,
                permissions: permissionsJson,
                createdBy: userId
            );

            var created = await _repository.CreateAsync(definition);

            return await _queryService.GetByIdAsync(created.Id);
        }
    }
}
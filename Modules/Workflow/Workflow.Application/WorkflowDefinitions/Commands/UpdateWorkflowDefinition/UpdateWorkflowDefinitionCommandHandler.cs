using Newtonsoft.Json;
using MediatR;
using Shared.Application.DTOs.Workflows;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Application.WorkflowDefinitions.Queries;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.UpdateWorkflowDefinition
{
    public class UpdateWorkflowDefinitionCommandHandler : IRequestHandler<UpdateWorkflowDefinitionCommand, ViewDetailWorkflowDefinitionDto?>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly IWorkflowDefinitionQueryService _queryService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateWorkflowDefinitionCommandHandler(
            IWorkflowDefinitionRepository repository,
            IWorkflowDefinitionQueryService queryService,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUserService = currentUserService;
        }

        public async Task<ViewDetailWorkflowDefinitionDto?> Handle(UpdateWorkflowDefinitionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var data = request.Data;

            var definition = await _repository.GetByIdAsync(request.Id);
            if (definition == null)
            {
                throw new NotFoundException("Không tìm thấy quy trình.");
            }

            if (await _repository.IsCodeExistsAsync(data.Code, request.Id))
            {
                throw new DomainException($"Mã quy trình '{data.Code}' đã tồn tại.");
            }

            var permissionsJson = JsonConvert.SerializeObject(new WorkflowPermissionsJsonDto
            {
                CreatePermissions = data.CreatePermissions,
                ViewPermissions = data.ViewPermissions
            });

            definition.Update(
                name: data.Name,
                code: data.Code,
                categoryId: data.CategoryId,
                icon: data.Icon,
                description: data.Description,
                permissions: permissionsJson,
                modifiedBy: userId
            );

            await _repository.UpdateAsync(definition);

            return await _queryService.GetByIdAsync(definition.Id);
        }
    }
}
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Application.WorkflowCategories.Queries;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowCategories;

namespace Workflow.Application.WorkflowCategories.Commands.CreateWorkflowCategory
{
    public class CreateWorkflowCategoryCommandHandler
        : IRequestHandler<CreateWorkflowCategoryCommand, ViewDetailWorkflowCategoryDto?>
    {
        private readonly IWorkflowCategoryRepository _repository;
        private readonly IWorkflowCategoryQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public CreateWorkflowCategoryCommandHandler(
            IWorkflowCategoryRepository repository,
            IWorkflowCategoryQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailWorkflowCategoryDto?> Handle(
            CreateWorkflowCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = WorkflowCategory.Create(
                name: dto.Name,
                description: dto.Description,
                icon: dto.Icon,
                createdBy: _currentUser.UserId
            );

            var created = await _repository.CreateAsync(entity);

            return await _queryService.GetByIdAsync(created.Id);
        }
    }
}
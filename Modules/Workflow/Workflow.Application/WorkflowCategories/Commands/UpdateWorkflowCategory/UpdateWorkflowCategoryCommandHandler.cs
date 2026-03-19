using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Application.WorkflowCategories.Queries;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowCategories.Commands.UpdateWorkflowCategory
{
    public class UpdateWorkflowCategoryCommandHandler
        : IRequestHandler<UpdateWorkflowCategoryCommand, ViewDetailWorkflowCategoryDto?>
    {
        private readonly IWorkflowCategoryRepository _repository;
        private readonly IWorkflowCategoryQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public UpdateWorkflowCategoryCommandHandler(
            IWorkflowCategoryRepository repository,
            IWorkflowCategoryQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailWorkflowCategoryDto?> Handle(
            UpdateWorkflowCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("Danh mục quy trình không tồn tại.");

            var dto = request.Dto;
            entity.Update(
                name: dto.Name,
                description: dto.Description,
                icon: dto.Icon,
                modifiedBy: _currentUser.UserId
            );

            await _repository.UpdateAsync(entity);

            return await _queryService.GetByIdAsync(entity.Id);
        }
    }
}
using Shared.Application.DTOs.Workflows;
using Workflow.Domain.WorkflowCategories;

namespace Workflow.Application.WorkflowCategories.Mappings
{
    public static class WorkflowCategoryMapping
    {
        #region ===================== VIEW LIST =====================

        public static ViewListWorkflowCategoryDto? ToListDto(this WorkflowCategory entity)
        {
            if (entity == null) return null;

            return new ViewListWorkflowCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Icon = entity.Icon,
                CreatedById = entity.CreatedBy,
                CreatedAt = entity.CreatedAt
            };
        }

        #endregion

        #region ===================== VIEW DETAIL =====================

        public static ViewDetailWorkflowCategoryDto? ToDetailDto(this WorkflowCategory entity)
        {
            if (entity == null) return null;

            return new ViewDetailWorkflowCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Icon = entity.Icon,
                CreatedById = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
                ModifiedById = entity.ModifiedBy,
                ModifiedAt = entity.ModifiedAt
            };
        }

        #endregion

        #region ===================== CREATE =====================

        public static WorkflowCategory ToCreateEntity(
            this CreateWorkflowCategoryDto dto,
            int createdBy)
        {
            return WorkflowCategory.Create(
                name: dto.Name,
                description: dto.Description,
                icon: dto.Icon,
                createdBy: createdBy
            );
        }

        #endregion

        #region ===================== UPDATE =====================

        public static WorkflowCategory ToUpdateEntity(
            this WorkflowCategory entity,
            UpdateWorkflowCategoryDto dto,
            int updatedBy)
        {
            entity.Update(
                name: dto.Name,
                description: dto.Description,
                icon: dto.Icon,
                modifiedBy: updatedBy
            );

            return entity;
        }

        #endregion
    }
}
using Identity.Domain.Departments;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Departments.Mappings
{
    public static class DepartmentMapping
    {
        public static ViewListDepartmentDto? ToListDto(this Department entity)
        {
            if (entity == null) return null;

            return new ViewListDepartmentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Index = entity.Index,
                Level = entity.Level,
                PathCode = entity.PathCode,
                ParentId = entity.ParentId,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy
            };
        }

        public static ViewDetailDepartmentDto? ToDetailDto(this Department entity)
        {
            if (entity == null) return null;

            return new ViewDetailDepartmentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Index = entity.Index,
                Level = entity.Level,
                PathCode = entity.PathCode,
                ParentId = entity.ParentId,
                ParentName = entity.Parent?.Name,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                ModifiedAt = entity.ModifiedAt,
                ModifiedBy = entity.ModifiedBy,
                Childrens = entity.Childrens
                    .Select(x => x.ToListDto()!)
                    .ToList()
            };
        }

        public static Department ToCreateEntity(
            this CreateDepartmentDto dto,
            int createdBy,
            int level,
            string pathCode)
        {
            return Department.Create(
                name: dto.Name,
                description: dto.Description,
                index: dto.Index,
                level: level,
                pathCode: pathCode,
                createdBy: createdBy,
                parentId: dto.ParentId
            );
        }
    }
}

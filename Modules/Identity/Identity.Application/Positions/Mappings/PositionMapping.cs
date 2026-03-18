using Identity.Domain.Positions;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Positions.Mappings
{
    public static class PositionMapping
    {
        public static ViewListPositionDto? ToListDto(this Position entity)
        {
            if (entity == null) return null;

            return new ViewListPositionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy
            };
        }

        public static ViewDetailPositionDto? ToDetailDto(this Position entity)
        {
            if (entity == null) return null;

            return new ViewDetailPositionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                ModifiedAt = entity.ModifiedAt,
                ModifiedBy = entity.ModifiedBy
            };
        }

        public static Position ToCreateEntity(
            this CreatePositionDto dto,
            int createdBy)
        {
            return Position.Create(
                name: dto.Name,
                description: dto.Description,
                createdBy: createdBy
            );
        }
    }
}

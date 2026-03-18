using Identity.Domain.Groups;
using Shared.Application.DTOs.Identity;
using System.Linq;

namespace Identity.Application.Groups.Mappings
{
    public static class GroupMapping
    {
        public static ViewListGroupDto? ToListDto(this Group entity)
        {
            if (entity == null) return null;

            return new ViewListGroupDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                UserCount = entity.UserGroups.Count,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy
            };
        }

        public static ViewDetailGroupDto? ToDetailDto(this Group entity)
        {
            if (entity == null) return null;

            return new ViewDetailGroupDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                ModifiedAt = entity.ModifiedAt,
                ModifiedBy = entity.ModifiedBy,
                Users = entity.UserGroups
                    .Where(ug => ug.User != null)
                    .Select(ug => new GroupUserDetailDto
                    {
                        UserId = ug.UserId,
                        Username = ug.User!.Username,
                        FullName = ug.User!.FullName,
                        Email = ug.User!.Email
                    })
                    .ToList()
            };
        }

        public static Group ToCreateEntity(
            this CreateGroupDto dto,
            int createdBy)
        {
            return Group.Create(
                name: dto.Name,
                description: dto.Description,
                createdBy: createdBy
            );
        }
    }
}

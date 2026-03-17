using Shared.Application.DTOs.Identity;
using System.Linq;
using Identity.Domain.Users;

namespace Identity.Application.Users.Mappings
{
    public static class UserMapping
    {
        #region ===================== VIEW LIST =====================

        public static ViewListUserDto? ToListDto(this User entity)
        {
            if (entity == null) return null;

            return new ViewListUserDto
            {
                Id = entity.Id,

                Username = entity.Username,
                FullName = entity.FullName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,

                DepartmentId = entity.DepartmentId,
                Department = entity.Department?.Name,

                PositionId = entity.PositionId,
                Position = entity.Position?.Name,

                IsActive = entity.IsActive,

                Gender = entity.Gender,
                DateOfBirth = entity.DateOfBirth,

                LastLoginAt = entity.LastLoginAt,

                CreatedById = entity.CreatedBy,
                CreatedAt = entity.CreatedAt
            };
        }

        #endregion

        #region ===================== VIEW DETAIL =====================

        public static ViewDetailUserDto? ToDetailDto(this User entity)
        {
            if (entity == null) return null;

            return new ViewDetailUserDto
            {
                Id = entity.Id,

                Username = entity.Username,
                FullName = entity.FullName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,

                Address = entity.Address,
                ImageUrl = entity.ImageUrl,

                Gender = entity.Gender,
                DateOfBirth = entity.DateOfBirth,

                DepartmentId = entity.DepartmentId,
                Department = entity.Department?.Name,

                PositionId = entity.PositionId,
                Position = entity.Position?.Name,

                IsActive = entity.IsActive,

                DistinguishedName = entity.DistinguishedName,
                Sid = entity.Sid,

                LastLoginAt = entity.LastLoginAt,
                LastSyncAt = entity.LastSyncAt,

                CreatedById = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,

                ModifiedById = entity.ModifiedBy,
                ModifiedAt = entity.ModifiedAt,

                Groups = entity.UserGroups
                    .Select(x => new UserGroupDto
                    {
                        GroupId = x.GroupId,
                        GroupName = x.Group?.Name
                    })
                    .ToList()
            };
        }

        #endregion

        #region ===================== CREATE =====================

        public static User ToCreateEntity(
            this CreateUserDto dto,
            int createdBy)
        {
            return User.Create(
                username: dto.Username,
                password: dto.Password,
                fullName: dto.FullName,
                email: dto.Email,
                createdBy: createdBy,
                phoneNumber: dto.PhoneNumber,
                gender: dto.Gender,
                dateOfBirth: dto.DateOfBirth,
                address: dto.Address,
                departmentId: dto.DepartmentId,
                positionId: dto.PositionId,
                imageUrl: dto.ImageUrl,
                isActive: dto.IsActive,
                distinguishedName: dto.DistinguishedName,
                sid: dto.Sid
            );
        }

        #endregion

        #region ===================== UPDATE =====================

        public static User ToUpdateEntity(
            this User entity,
            UpdateUserDto dto,
            int updatedBy)
        {
            entity.Update(
                password: dto.Password,
                fullName: dto.FullName,
                email: dto.Email,
                modifiedBy: updatedBy,
                phoneNumber: dto.PhoneNumber,
                gender: dto.Gender,
                dateOfBirth: dto.DateOfBirth,
                address: dto.Address,
                departmentId: dto.DepartmentId,
                positionId: dto.PositionId,
                imageUrl: dto.ImageUrl,
                isActive: dto.IsActive,
                distinguishedName: dto.DistinguishedName,
                sid: dto.Sid
            );

            return entity;
        }

        #endregion
    }
}
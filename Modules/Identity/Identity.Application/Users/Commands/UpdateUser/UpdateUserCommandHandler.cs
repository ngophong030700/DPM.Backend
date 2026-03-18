using Identity.Application.Users.Mappings;
using Identity.Application.Users.Queries;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Identity;
using Shared.Domain.Exceptions;

namespace Identity.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, ViewDetailUserDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserQueryService _userQueryService;
        private readonly ICurrentUserService _currentUser;
        private readonly IFileStorageService _fileStorage;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            IUserQueryService userQueryService,
            ICurrentUserService currentUser,
            IFileStorageService fileStorage)
        {
            _userRepository = userRepository;
            _userQueryService = userQueryService;
            _currentUser = currentUser;
            _fileStorage = fileStorage;
        }

        public async Task<ViewDetailUserDto?> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 🔹 Get entity
            var entity = await _userRepository.GetUserByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("User không tồn tại hoặc đã bị xóa.");

            // ... (email validation) ...

            // 🔹 Handle Avatar upload
            string? imageUrl = dto.ImageUrl;
            if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("data:image"))
            {
                // Delete old file if exists
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    await _fileStorage.DeleteFileAsync(entity.ImageUrl);
                }

                var (logicalPath, _, _, _) = await _fileStorage.UploadFileAsync(imageUrl, $"{entity.Username}_avatar");
                imageUrl = logicalPath;
            }

            // 🔹 Mapping update
            entity.Update(
                password: dto.Password,
                fullName: dto.FullName,
                email: dto.Email,
                modifiedBy: _currentUser.UserId,
                phoneNumber: dto.PhoneNumber,
                gender: dto.Gender,
                dateOfBirth: dto.DateOfBirth,
                address: dto.Address,
                departmentId: dto.DepartmentId,
                positionId: dto.PositionId,
                imageUrl: imageUrl, // Sử dụng logical path đã upload
                isActive: dto.IsActive,
                distinguishedName: dto.DistinguishedName,
                sid: dto.Sid
            );

            // 🔹 Update
            var updated = await _userRepository.UpdateUserAsync(entity);

            if (updated == null)
                throw new DomainException("Không thể cập nhật user.");

            // 🔹 Handle Group associations
            if (dto.GroupIds != null)
            {
                await _userRepository.ClearGroupsForUserAsync(request.Id);
                foreach (var groupId in dto.GroupIds)
                {
                    await _userRepository.AddUserToGroupAsync(request.Id, groupId, _currentUser.UserId);
                }
            }

            // Refresh to get full detail with groups and Names
            return await _userQueryService.GetUserByIdAsync(request.Id);
        }
    }
}
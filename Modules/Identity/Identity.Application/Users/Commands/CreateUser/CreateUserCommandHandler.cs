using Identity.Application.Users.Mappings;
using Identity.Application.Users.Queries;
using Identity.Domain.Repositories;
using Identity.Domain.Users;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Identity;
using Shared.Domain.Exceptions;

namespace Identity.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, ViewDetailUserDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserQueryService _userQueryService;
        private readonly ICurrentUserService _currentUser;
        private readonly IFileStorageService _fileStorage;

        public CreateUserCommandHandler(
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
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // ... (duplicate checks) ...

            // 🔹 Handle Avatar upload
            string? imageUrl = dto.ImageUrl;
            if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("data:image"))
            {
                var (logicalPath, _, _, _) = await _fileStorage.UploadFileAsync(imageUrl, $"{dto.Username}_avatar");
                imageUrl = logicalPath;
            }

            // 🔹 Mapping
            var entity = User.Create(
                username: dto.Username,
                password: dto.Password,
                fullName: dto.FullName,
                email: dto.Email,
                createdBy: _currentUser.UserId,
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

            // 🔹 Create
            var created = await _userRepository.CreateUserAsync(entity);

            if (created == null)
                throw new DomainException("Không thể tạo user.");

            // 🔹 Handle Group associations
            if (dto.GroupIds != null && dto.GroupIds.Any())
            {
                foreach (var groupId in dto.GroupIds)
                {
                    await _userRepository.AddUserToGroupAsync(created.Id, groupId, _currentUser.UserId);
                }
            }

            // Refresh to get full detail with groups and Names
            return await _userQueryService.GetUserByIdAsync(created.Id);
        }
    }
}
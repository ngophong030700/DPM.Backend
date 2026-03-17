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

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            IUserQueryService userQueryService,
            ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _userQueryService = userQueryService;
            _currentUser = currentUser;
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

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var exists = await _userQueryService
                    .IsEmailExistsAsync(dto.Email, request.Id);

                if (exists)
                    throw new DuplicateException($"Email '{dto.Email}' đã tồn tại.");
            }

            // 🔹 Mapping update
            entity = entity.ToUpdateEntity(dto, _currentUser.UserId);

            // 🔹 Update
            var updated = await _userRepository.UpdateUserAsync(entity);

            if (updated == null)
                throw new DomainException("Không thể cập nhật user.");

            return updated.ToDetailDto();
        }
    }
}
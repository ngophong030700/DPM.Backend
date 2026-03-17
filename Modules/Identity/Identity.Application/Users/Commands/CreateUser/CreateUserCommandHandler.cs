using Identity.Application.Users.Mappings;
using Identity.Application.Users.Queries;
using Identity.Domain.Repositories;
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

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IUserQueryService userQueryService,
            ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _userQueryService = userQueryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailUserDto?> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 🔹 Check duplicate
            if (await _userQueryService.IsUsernameExistsAsync(dto.Username))
                throw new DuplicateException($"Username '{dto.Username}' đã tồn tại.");

            if (!string.IsNullOrWhiteSpace(dto.Email) &&
                await _userQueryService.IsEmailExistsAsync(dto.Email))
                throw new DuplicateException($"Email '{dto.Email}' đã tồn tại.");

            // 🔹 Mapping
            var entity = dto.ToCreateEntity(_currentUser.UserId);

            // 🔹 Create
            var created = await _userRepository.CreateUserAsync(entity);

            if (created == null)
                throw new DomainException("Không thể tạo user.");

            return created.ToDetailDto();
        }
    }
}
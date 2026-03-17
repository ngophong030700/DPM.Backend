using Identity.Application.Users.Queries;
using Identity.Domain.Repositories;
using Identity.Domain.Users;
using MediatR;
using Shared.Application.DTOs.Identity;
using Shared.Domain.Exceptions;
using Shared.Infrastructure.Services;

namespace Identity.Application.Users.Commands.Login
{
    public class UserLoginCommandHandler
        : IRequestHandler<UserLoginCommand, UserLoginResultDto>
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserLoginCommandHandler(
            IUserQueryService userQueryService,
            IUserRepository userRepository,
            IJwtService jwtService)
        {
            _userQueryService = userQueryService;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<UserLoginResultDto> Handle(
            UserLoginCommand request,
            CancellationToken cancellationToken)
        {
            // 🔹 1. Get user (READ)
            var user = await _userRepository
                .GetUserByUsernameAsync(request.Username);

            if (user == null)
                throw new NotFoundException("User không tồn tại.");

            if (user.IsDeleted)
                throw new DomainException("Tài khoản đã bị xóa.");

            if (!user.IsActive)
                throw new DomainException("Tài khoản đang bị khóa.");

            // 🔹 2. Validate password
            if (!user.VerifyPassword(request.Password))
                throw new DomainException("Sai username hoặc password.");

            // 🔹 3. Update last login (WRITE)
            user.SetLastLoginAt(DateTime.UtcNow);

            await _userRepository.UpdateUserAsync(user);

            // 🔹 4. Generate JWT
            var token = _jwtService.GenerateToken(user);

            // 🔹 5. Return DTO
            return new UserLoginResultDto
            {
                Token = token,
                Username = user.Username,
                FullName = user.FullName ?? "",
                Email = user.Email ?? ""
            };
        }
    }
}
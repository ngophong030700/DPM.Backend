using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;

namespace Identity.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler
        : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;

        public DeleteUserCommandHandler(
            IUserRepository userRepository,
            ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            return await _userRepository
                .SoftDeleteUserAsync(request.Id, _currentUser.UserId);
        }
    }
}
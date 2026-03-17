using MediatR;

namespace Identity.Application.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(int Id) : IRequest<bool>;
}
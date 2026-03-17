using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(CreateUserDto Dto)
        : IRequest<ViewDetailUserDto?>;
}
using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Users.Commands.Login
{
    public record UserLoginCommand(string Username, string Password)
        : IRequest<UserLoginResultDto>;
}
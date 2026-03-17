using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(int Id, UpdateUserDto Dto)
        : IRequest<ViewDetailUserDto?>;
}
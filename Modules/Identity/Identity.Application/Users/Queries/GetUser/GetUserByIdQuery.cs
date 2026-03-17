using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Users.Queries.GetUser
{
    public record GetUserByIdQuery(int Id)
        : IRequest<ViewDetailUserDto?>;
}
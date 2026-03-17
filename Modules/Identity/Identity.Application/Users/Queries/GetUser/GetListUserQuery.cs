using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Users.Queries.GetUser
{
    public record GetListUserQuery(PagingRequest Request)
        : IRequest<PagingResponse<ViewListUserDto>>;
}
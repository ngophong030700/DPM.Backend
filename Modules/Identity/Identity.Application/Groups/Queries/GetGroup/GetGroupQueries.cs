using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Groups.Queries.GetGroup
{
    public record GetListGroupQuery(PagingRequest Request)
        : IRequest<PagingResponse<ViewListGroupDto>>;

    public record GetGroupByIdQuery(int Id)
        : IRequest<ViewDetailGroupDto?>;
}

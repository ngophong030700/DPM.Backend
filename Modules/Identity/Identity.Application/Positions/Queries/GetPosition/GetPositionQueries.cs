using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Positions.Queries.GetPosition
{
    public record GetListPositionQuery(PagingRequest Request)
        : IRequest<PagingResponse<ViewListPositionDto>>;

    public record GetPositionByIdQuery(int Id)
        : IRequest<ViewDetailPositionDto?>;
}

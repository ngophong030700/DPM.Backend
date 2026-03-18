using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Positions.Queries.GetPosition
{
    public record GetListPositionQuery(string? Keyword)
        : IRequest<IEnumerable<ViewListPositionDto>>;

    public record GetPositionByIdQuery(int Id)
        : IRequest<ViewDetailPositionDto?>;
}

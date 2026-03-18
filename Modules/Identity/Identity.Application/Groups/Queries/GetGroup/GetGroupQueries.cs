using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Groups.Queries.GetGroup
{
    public record GetListGroupQuery(string? Keyword)
        : IRequest<IEnumerable<ViewListGroupDto>>;

    public record GetGroupByIdQuery(int Id)
        : IRequest<ViewDetailGroupDto?>;
}

using Identity.Application.Groups.Mappings;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Groups.Queries.GetGroup
{
    public class GetGroupQueriesHandler
        : IRequestHandler<GetListGroupQuery, IEnumerable<ViewListGroupDto>>,
          IRequestHandler<GetGroupByIdQuery, ViewDetailGroupDto?>
    {
        private readonly IGroupQueryService _queryService;

        public GetGroupQueriesHandler(IGroupQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<IEnumerable<ViewListGroupDto>> Handle(
            GetListGroupQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetListAsync(request.Keyword);
        }

        public async Task<ViewDetailGroupDto?> Handle(
            GetGroupByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetByIdAsync(request.Id);
        }
    }
}

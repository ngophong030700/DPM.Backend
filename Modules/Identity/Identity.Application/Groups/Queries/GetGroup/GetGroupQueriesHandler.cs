using Identity.Application.Groups.Mappings;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Groups.Queries.GetGroup
{
    public class GetGroupQueriesHandler
        : IRequestHandler<GetListGroupQuery, PagingResponse<ViewListGroupDto>>,
          IRequestHandler<GetGroupByIdQuery, ViewDetailGroupDto?>
    {
        private readonly IGroupQueryService _queryService;

        public GetGroupQueriesHandler(IGroupQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<PagingResponse<ViewListGroupDto>> Handle(
            GetListGroupQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetListAsync(request.Request);
        }

        public async Task<ViewDetailGroupDto?> Handle(
            GetGroupByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetByIdAsync(request.Id);
        }
    }
}

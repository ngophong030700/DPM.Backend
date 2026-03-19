using Identity.Application.Positions.Mappings;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Positions.Queries.GetPosition
{
    public class GetPositionQueriesHandler
        : IRequestHandler<GetListPositionQuery, PagingResponse<ViewListPositionDto>>,
          IRequestHandler<GetPositionByIdQuery, ViewDetailPositionDto?>
    {
        private readonly IPositionQueryService _queryService;

        public GetPositionQueriesHandler(IPositionQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<PagingResponse<ViewListPositionDto>> Handle(
            GetListPositionQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetListAsync(request.Request);
        }

        public async Task<ViewDetailPositionDto?> Handle(
            GetPositionByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetByIdAsync(request.Id);
        }
    }
}

using Identity.Application.Departments.Mappings;
using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Departments.Queries.GetDepartment
{
    public class GetDepartmentQueriesHandler
        : IRequestHandler<GetListDepartmentQuery, PagingResponse<ViewListDepartmentDto>>,
          IRequestHandler<GetDepartmentByIdQuery, ViewDetailDepartmentDto?>
    {
        private readonly IDepartmentQueryService _queryService;

        public GetDepartmentQueriesHandler(IDepartmentQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<PagingResponse<ViewListDepartmentDto>> Handle(
            GetListDepartmentQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetListAsync(request.Request);
        }

        public async Task<ViewDetailDepartmentDto?> Handle(
            GetDepartmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetByIdAsync(request.Id);
        }
    }
}

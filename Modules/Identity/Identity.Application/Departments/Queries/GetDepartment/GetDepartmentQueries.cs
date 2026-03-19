using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Departments.Queries.GetDepartment
{
    public record GetListDepartmentQuery(PagingRequest Request)
        : IRequest<PagingResponse<ViewListDepartmentDto>>;

    public record GetDepartmentByIdQuery(int Id)
        : IRequest<ViewDetailDepartmentDto?>;
}

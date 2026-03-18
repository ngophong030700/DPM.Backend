using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Departments.Queries.GetDepartment
{
    public record GetListDepartmentQuery(string? Keyword)
        : IRequest<IEnumerable<ViewListDepartmentDto>>;

    public record GetDepartmentByIdQuery(int Id)
        : IRequest<ViewDetailDepartmentDto?>;
}

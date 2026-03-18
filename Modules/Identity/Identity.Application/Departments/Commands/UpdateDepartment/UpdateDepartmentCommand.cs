using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Departments.Commands.UpdateDepartment
{
    public record UpdateDepartmentCommand(int Id, UpdateDepartmentDto Dto)
        : IRequest<ViewDetailDepartmentDto?>;
}

using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Departments.Commands.CreateDepartment
{
    public record CreateDepartmentCommand(CreateDepartmentDto Dto)
        : IRequest<ViewDetailDepartmentDto?>;
}

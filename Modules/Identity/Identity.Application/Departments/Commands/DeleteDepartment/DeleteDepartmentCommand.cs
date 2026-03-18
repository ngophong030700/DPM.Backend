using MediatR;

namespace Identity.Application.Departments.Commands.DeleteDepartment
{
    public record DeleteDepartmentCommand(int Id) : IRequest<bool>;
}

using MediatR;

namespace Identity.Application.Groups.Commands.DeleteGroup
{
    public record DeleteGroupCommand(int Id) : IRequest<bool>;
}

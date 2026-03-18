using MediatR;

namespace Identity.Application.Positions.Commands.DeletePosition
{
    public record DeletePositionCommand(int Id) : IRequest<bool>;
}

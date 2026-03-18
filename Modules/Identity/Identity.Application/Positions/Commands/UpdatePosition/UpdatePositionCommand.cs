using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Positions.Commands.UpdatePosition
{
    public record UpdatePositionCommand(int Id, UpdatePositionDto Dto)
        : IRequest<ViewDetailPositionDto?>;
}

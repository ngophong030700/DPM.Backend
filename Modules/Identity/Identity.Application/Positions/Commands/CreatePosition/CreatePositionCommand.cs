using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Positions.Commands.CreatePosition
{
    public record CreatePositionCommand(CreatePositionDto Dto)
        : IRequest<ViewDetailPositionDto?>;
}

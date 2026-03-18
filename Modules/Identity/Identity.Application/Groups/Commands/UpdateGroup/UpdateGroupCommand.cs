using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Groups.Commands.UpdateGroup
{
    public record UpdateGroupCommand(int Id, UpdateGroupDto Dto)
        : IRequest<ViewDetailGroupDto?>;
}

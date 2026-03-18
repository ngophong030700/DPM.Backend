using MediatR;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Groups.Commands.CreateGroup
{
    public record CreateGroupCommand(CreateGroupDto Dto)
        : IRequest<ViewDetailGroupDto?>;
}

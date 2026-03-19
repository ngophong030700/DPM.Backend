using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.MasterDataSources.Commands.UpdateMasterDataValue
{
    public record UpdateMasterDataValueCommand(int ValueId, UpdateMasterDataValueDto Dto)
        : IRequest<ViewMasterDataValueDto?>;
}
using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.MasterDataSources.Commands.AddMasterDataValue
{
    public record AddMasterDataValueCommand(int SourceId, CreateMasterDataValueDto Dto)
        : IRequest<ViewMasterDataValueDto?>;
}
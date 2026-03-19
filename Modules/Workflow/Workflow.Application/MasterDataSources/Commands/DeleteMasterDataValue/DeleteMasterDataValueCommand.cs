using MediatR;

namespace Workflow.Application.MasterDataSources.Commands.DeleteMasterDataValue
{
    public record DeleteMasterDataValueCommand(int ValueId) : IRequest<bool>;
}
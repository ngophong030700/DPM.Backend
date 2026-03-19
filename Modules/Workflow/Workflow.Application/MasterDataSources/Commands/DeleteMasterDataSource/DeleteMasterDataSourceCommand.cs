using MediatR;

namespace Workflow.Application.MasterDataSources.Commands.DeleteMasterDataSource
{
    public record DeleteMasterDataSourceCommand(int Id) : IRequest<bool>;
}
using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.MasterDataSources.Commands.CreateMasterDataSource
{
    public record CreateMasterDataSourceCommand(CreateMasterDataSourceDto Dto)
        : IRequest<ViewDetailMasterDataSourceDto?>;
}
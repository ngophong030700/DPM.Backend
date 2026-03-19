using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.MasterDataSources.Commands.UpdateMasterDataSource
{
    public record UpdateMasterDataSourceCommand(int Id, UpdateMasterDataSourceDto Dto)
        : IRequest<ViewDetailMasterDataSourceDto?>;
}
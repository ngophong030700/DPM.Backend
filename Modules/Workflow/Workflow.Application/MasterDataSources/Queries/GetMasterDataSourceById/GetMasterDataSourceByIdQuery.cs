using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.MasterDataSources.Queries.GetMasterDataSourceById
{
    public record GetMasterDataSourceByIdQuery(int Id) : IRequest<ViewDetailMasterDataSourceDto?>;
}
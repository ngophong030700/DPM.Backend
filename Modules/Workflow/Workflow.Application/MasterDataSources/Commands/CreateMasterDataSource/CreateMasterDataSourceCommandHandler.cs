using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Application.MasterDataSources.Queries;
using Workflow.Domain.MasterDataSources;
using Workflow.Domain.Repositories;

namespace Workflow.Application.MasterDataSources.Commands.CreateMasterDataSource
{
    public class CreateMasterDataSourceCommandHandler
        : IRequestHandler<CreateMasterDataSourceCommand, ViewDetailMasterDataSourceDto?>
    {
        private readonly IMasterDataSourceRepository _repository;
        private readonly IMasterDataSourceQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public CreateMasterDataSourceCommandHandler(
            IMasterDataSourceRepository repository,
            IMasterDataSourceQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailMasterDataSourceDto?> Handle(
            CreateMasterDataSourceCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            if (await _repository.IsCodeExistsAsync(dto.Code))
                throw new DuplicateException($"Mã Master Data Source '{dto.Code}' đã tồn tại.");

            var entity = MasterDataSource.Create(
                name: dto.Name,
                code: dto.Code,
                description: dto.Description,
                createdBy: _currentUser.UserId
            );

            if (dto.Columns != null && dto.Columns.Any())
            {
                foreach (var colDto in dto.Columns)
                {
                    var col = MasterDataColumn.Create(
                        sourceId: 0, // Will be handled by EF relation
                        columnKey: colDto.ColumnKey,
                        columnLabel: colDto.ColumnLabel,
                        dataType: colDto.DataType,
                        isRequired: colDto.IsRequired,
                        sortOrder: colDto.SortOrder,
                        createdBy: _currentUser.UserId
                    );
                    entity.AddColumn(col);
                }
            }

            var created = await _repository.CreateAsync(entity);

            return await _queryService.GetByIdAsync(created.Id);
        }
    }
}
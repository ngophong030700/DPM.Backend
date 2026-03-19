using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Application.MasterDataSources.Queries;
using Workflow.Domain.MasterDataSources;
using Workflow.Domain.Repositories;

namespace Workflow.Application.MasterDataSources.Commands.AddMasterDataValue
{
    public class AddMasterDataValueCommandHandler
        : IRequestHandler<AddMasterDataValueCommand, ViewMasterDataValueDto?>
    {
        private readonly IMasterDataSourceRepository _repository;
        private readonly IMasterDataSourceQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public AddMasterDataValueCommandHandler(
            IMasterDataSourceRepository repository,
            IMasterDataSourceQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewMasterDataValueDto?> Handle(
            AddMasterDataValueCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var source = await _repository.GetByIdAsync(request.SourceId);
            if (source == null)
                throw new NotFoundException("Nguồn dữ liệu không tồn tại.");

            // Create MasterDataValue
            var value = MasterDataValue.Create(
                sourceId: source.Id,
                displayName: dto.DisplayName,
                valueCode: dto.ValueCode,
                sortOrder: dto.SortOrder,
                createdBy: _currentUser.UserId
            );

            // Add cells based on source columns
            foreach (var column in source.Columns)
            {
                string? cellValue = null;
                if (dto.Cells.ContainsKey(column.ColumnKey))
                {
                    cellValue = dto.Cells[column.ColumnKey];
                }

                var cell = MasterDataCell.Create(
                    valueId: 0, // Will be handled by EF relation
                    columnId: column.Id,
                    cellValue: cellValue,
                    createdBy: _currentUser.UserId
                );
                value.AddCell(cell);
            }

            await _repository.AddValueAsync(value);
            await _repository.SaveChangesAsync();

            return await _queryService.GetValueByIdAsync(value.Id);
        }
    }
}
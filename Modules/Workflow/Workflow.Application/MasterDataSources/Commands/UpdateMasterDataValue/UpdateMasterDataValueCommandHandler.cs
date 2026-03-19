using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Application.MasterDataSources.Queries;
using Workflow.Domain.Repositories;

namespace Workflow.Application.MasterDataSources.Commands.UpdateMasterDataValue
{
    public class UpdateMasterDataValueCommandHandler
        : IRequestHandler<UpdateMasterDataValueCommand, ViewMasterDataValueDto?>
    {
        private readonly IMasterDataSourceRepository _repository;
        private readonly IMasterDataSourceQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public UpdateMasterDataValueCommandHandler(
            IMasterDataSourceRepository repository,
            IMasterDataSourceQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewMasterDataValueDto?> Handle(
            UpdateMasterDataValueCommand request,
            CancellationToken cancellationToken)
        {
            var value = await _repository.GetValueByIdAsync(request.ValueId);
            if (value == null)
                throw new NotFoundException("Dòng dữ liệu không tồn tại.");

            var source = await _repository.GetByIdAsync(value.SourceId);
            if (source == null)
                throw new NotFoundException("Nguồn dữ liệu không tồn tại.");

            var dto = request.Dto;

            // Update MasterDataValue
            value.Update(
                displayName: dto.DisplayName,
                valueCode: dto.ValueCode,
                sortOrder: dto.SortOrder,
                isActive: dto.IsActive,
                modifiedBy: _currentUser.UserId
            );

            // Update cells
            foreach (var column in source.Columns)
            {
                if (dto.Cells.ContainsKey(column.ColumnKey))
                {
                    value.UpdateOrAddCell(column.Id, dto.Cells[column.ColumnKey], _currentUser.UserId);
                }
            }

            _repository.UpdateValue(value);
            await _repository.SaveChangesAsync();

            return await _queryService.GetValueByIdAsync(value.Id);
        }
    }
}
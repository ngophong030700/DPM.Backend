using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Application.MasterDataSources.Queries;
using Workflow.Domain.MasterDataSources;
using Workflow.Domain.Repositories;

namespace Workflow.Application.MasterDataSources.Commands.UpdateMasterDataSource
{
    public class UpdateMasterDataSourceCommandHandler
        : IRequestHandler<UpdateMasterDataSourceCommand, ViewDetailMasterDataSourceDto?>
    {
        private readonly IMasterDataSourceRepository _repository;
        private readonly IMasterDataSourceQueryService _queryService;
        private readonly ICurrentUserService _currentUser;

        public UpdateMasterDataSourceCommandHandler(
            IMasterDataSourceRepository repository,
            IMasterDataSourceQueryService queryService,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _queryService = queryService;
            _currentUser = currentUser;
        }

        public async Task<ViewDetailMasterDataSourceDto?> Handle(
            UpdateMasterDataSourceCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("Nguồn dữ liệu không tồn tại.");

            var dto = request.Dto;

            if (await _repository.IsCodeExistsAsync(dto.Code, entity.Id))
                throw new DuplicateException($"Mã Master Data Source '{dto.Code}' đã được sử dụng.");

            // Update Source Info
            entity.Update(
                name: dto.Name,
                code: dto.Code,
                description: dto.Description,
                isActive: dto.IsActive,
                modifiedBy: _currentUser.UserId
            );

            // SYNC COLUMNS
            // Lấy tất cả cột bao gồm cả cột đã xóa mềm để có thể khôi phục nếu cần (hoặc khớp key)
            // Tuy nhiên repository GetByIdAsync hiện tại chỉ lấy cột chưa xóa. 
            // Để đơn giản và an toàn, ta làm việc trên danh sách hiện tại.
            var existingColumns = entity.Columns.ToList();
            
            // Xác định danh sách ID sẽ giữ lại
            var incomingColumnIds = dto.Columns
                .Where(c => c.Id.HasValue)
                .Select(c => c.Id!.Value)
                .ToList();

            // 1. Xử lý các cột không còn trong danh sách gửi lên -> Xóa mềm
            foreach (var existingCol in existingColumns)
            {
                // Nếu cột hiện tại không có trong danh sách ID gửi lên 
                // VÀ cũng không khớp ColumnKey (trường hợp frontend không gửi ID nhưng giữ nguyên Key)
                if (!incomingColumnIds.Contains(existingCol.Id) && 
                    !dto.Columns.Any(c => !c.Id.HasValue && c.ColumnKey == existingCol.ColumnKey))
                {
                    existingCol.SoftDelete(_currentUser.UserId);
                }
            }

            // 2. Thêm mới hoặc Cập nhật
            foreach (var colDto in dto.Columns)
            {
                MasterDataColumn? colToUpdate = null;

                if (colDto.Id.HasValue)
                {
                    colToUpdate = existingColumns.FirstOrDefault(c => c.Id == colDto.Id.Value);
                }
                else
                {
                    // Nếu không có ID, thử tìm theo ColumnKey để tránh tạo trùng và mất dữ liệu
                    colToUpdate = existingColumns.FirstOrDefault(c => c.ColumnKey == colDto.ColumnKey);
                }

                if (colToUpdate != null)
                {
                    // Cập nhật thông tin cột hiện có
                    colToUpdate.Update(
                        columnKey: colDto.ColumnKey,
                        columnLabel: colDto.ColumnLabel,
                        dataType: colDto.DataType,
                        isRequired: colDto.IsRequired,
                        sortOrder: colDto.SortOrder,
                        modifiedBy: _currentUser.UserId
                    );
                }
                else
                {
                    // Thêm cột mới hoàn toàn
                    var newCol = MasterDataColumn.Create(
                        sourceId: entity.Id,
                        columnKey: colDto.ColumnKey,
                        columnLabel: colDto.ColumnLabel,
                        dataType: colDto.DataType,
                        isRequired: colDto.IsRequired,
                        sortOrder: colDto.SortOrder,
                        createdBy: _currentUser.UserId
                    );
                    entity.AddColumn(newCol);
                }
            }

            await _repository.UpdateAsync(entity);

            return await _queryService.GetByIdAsync(entity.Id);
        }
    }
}
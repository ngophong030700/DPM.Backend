using Shared.Application.DTOs.Workflows;
using Workflow.Domain.MasterDataSources;

namespace Workflow.Application.MasterDataSources.Mappings
{
    public static class MasterDataSourceMapping
    {
        #region ===================== SOURCE =====================

        public static ViewListMasterDataSourceDto? ToListDto(this MasterDataSource entity)
        {
            if (entity == null) return null;

            return new ViewListMasterDataSourceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt
            };
        }

        public static ViewDetailMasterDataSourceDto? ToDetailDto(this MasterDataSource entity)
        {
            if (entity == null) return null;

            return new ViewDetailMasterDataSourceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                ModifiedAt = entity.ModifiedAt,
                ModifiedBy = entity.ModifiedBy,
                Columns = entity.Columns
                    .Where(c => !c.IsDeleted)
                    .OrderBy(c => c.SortOrder)
                    .Select(c => c.ToColumnDto()!)
                    .ToList()
            };
        }

        #endregion

        #region ===================== COLUMN =====================

        public static ViewMasterDataColumnDto? ToColumnDto(this MasterDataColumn entity)
        {
            if (entity == null) return null;

            return new ViewMasterDataColumnDto
            {
                Id = entity.Id,
                ColumnKey = entity.ColumnKey,
                ColumnLabel = entity.ColumnLabel,
                DataType = entity.DataType,
                IsRequired = entity.IsRequired,
                SortOrder = entity.SortOrder
            };
        }

        #endregion

        #region ===================== VALUE (ROW) =====================

        public static ViewMasterDataValueDto? ToValueDto(this MasterDataValue entity, List<MasterDataColumn> columns)
        {
            if (entity == null) return null;

            return new ViewMasterDataValueDto
            {
                Id = entity.Id,
                DisplayName = entity.DisplayName,
                ValueCode = entity.ValueCode,
                SortOrder = entity.SortOrder,
                IsActive = entity.IsActive,
                Cells = columns.ToDictionary(
                    c => c.ColumnKey,
                    c => entity.Cells.FirstOrDefault(cell => cell.ColumnId == c.Id)?.CellValue
                )
            };
        }

        #endregion
    }
}
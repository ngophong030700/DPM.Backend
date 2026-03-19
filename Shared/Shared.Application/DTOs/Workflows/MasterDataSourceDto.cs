using System.ComponentModel.DataAnnotations;

namespace Shared.Application.DTOs.Workflows
{
    #region ===================== SOURCE =====================

    public class ViewListMasterDataSourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ViewDetailMasterDataSourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<ViewMasterDataColumnDto> Columns { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
    }

    public class CreateMasterDataSourceDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public List<CreateMasterDataColumnDto> Columns { get; set; } = new();
    }

    public class UpdateMasterDataSourceDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public List<UpdateMasterDataColumnDto> Columns { get; set; } = new();
    }

    #endregion

    #region ===================== COLUMN =====================

    public class ViewMasterDataColumnDto
    {
        public int Id { get; set; }
        public string ColumnKey { get; set; } = string.Empty;
        public string ColumnLabel { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
    }

    public class CreateMasterDataColumnDto
    {
        [Required]
        public string ColumnKey { get; set; } = string.Empty;
        [Required]
        public string ColumnLabel { get; set; } = string.Empty;
        [Required]
        public string DataType { get; set; } = "string";
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
    }

    public class UpdateMasterDataColumnDto
    {
        public int? Id { get; set; } // Null if new column
        [Required]
        public string ColumnKey { get; set; } = string.Empty;
        [Required]
        public string ColumnLabel { get; set; } = string.Empty;
        [Required]
        public string DataType { get; set; } = "string";
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
    }

    #endregion

    #region ===================== VALUE (ROW) =====================

    public class ViewMasterDataValueDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string ValueCode { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public Dictionary<string, string?> Cells { get; set; } = new();
    }

    public class CreateMasterDataValueDto
    {
        [Required]
        public string DisplayName { get; set; } = string.Empty;
        [Required]
        public string ValueCode { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public Dictionary<string, string?> Cells { get; set; } = new();
    }

    public class UpdateMasterDataValueDto
    {
        [Required]
        public string DisplayName { get; set; } = string.Empty;
        [Required]
        public string ValueCode { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public Dictionary<string, string?> Cells { get; set; } = new();
    }

    #endregion
}
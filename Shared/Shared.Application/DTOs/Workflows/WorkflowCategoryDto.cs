using System.ComponentModel.DataAnnotations;

namespace Shared.Application.DTOs.Workflows
{
    #region ===================== VIEW LIST =====================

    public class ViewListWorkflowCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Icon { get; set; } = string.Empty;

        public int CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    #endregion

    #region ===================== VIEW DETAIL =====================

    public class ViewDetailWorkflowCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Icon { get; set; } = string.Empty;

        public int CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public int ModifiedById { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    #endregion

    #region ===================== CREATE =====================

    public class CreateWorkflowCategoryDto
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Icon là bắt buộc.")]
        [MaxLength(100)]
        public string Icon { get; set; } = string.Empty;
    }

    #endregion

    #region ===================== UPDATE =====================

    public class UpdateWorkflowCategoryDto
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Icon là bắt buộc.")]
        [MaxLength(100)]
        public string Icon { get; set; } = string.Empty;
    }

    #endregion
}
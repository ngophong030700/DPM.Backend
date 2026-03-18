using Shared.Application.BaseClass;
using System.ComponentModel.DataAnnotations;

namespace Shared.Application.DTOs.Identity
{
    #region ===================== VIEW LIST =====================

    public class ViewListPositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
    }

    #endregion

    #region ===================== VIEW DETAIL =====================

    public class ViewDetailPositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
    }

    #endregion

    #region ===================== CREATE =====================

    public class CreatePositionDto
    {
        [Required(ErrorMessage = "Tên chức vụ là bắt buộc.")]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    #endregion

    #region ===================== UPDATE =====================

    public class UpdatePositionDto
    {
        [Required(ErrorMessage = "Tên chức vụ là bắt buộc.")]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    #endregion
}

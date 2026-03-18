using Shared.Application.BaseClass;
using System.ComponentModel.DataAnnotations;

namespace Shared.Application.DTOs.Identity
{
    #region ===================== VIEW LIST =====================

    public class ViewListDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        
        public int Index { get; set; }
        public int Level { get; set; }
        public string PathCode { get; set; } = default!;
        public int? ParentId { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
    }

    #endregion

    #region ===================== VIEW DETAIL =====================

    public class ViewDetailDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        
        public int Index { get; set; }
        public int Level { get; set; }
        public string PathCode { get; set; } = default!;
        public int? ParentId { get; set; }
        public string? ParentName { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }

        public List<ViewListDepartmentDto> Childrens { get; set; } = new();
    }

    #endregion

    #region ===================== CREATE =====================

    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Tên phòng ban là bắt buộc.")]
        public string Name { get; set; } = default!;
        
        public string? Description { get; set; }
        
        public int? ParentId { get; set; }
        public int Index { get; set; } = 1;
    }

    #endregion

    #region ===================== UPDATE =====================

    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "Tên phòng ban là bắt buộc.")]
        public string Name { get; set; } = default!;
        
        public string? Description { get; set; }
        
        public int? ParentId { get; set; }
        public int Index { get; set; }
    }

    #endregion
}

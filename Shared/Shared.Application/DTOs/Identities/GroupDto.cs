using Shared.Application.BaseClass;
using System.ComponentModel.DataAnnotations;

namespace Shared.Application.DTOs.Identity
{
    #region ===================== VIEW LIST =====================

    public class ViewListGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public int UserCount { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
    }

    #endregion

    #region ===================== VIEW DETAIL =====================

    public class ViewDetailGroupDto
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

        public List<GroupUserDetailDto> Users { get; set; } = new();
    }

    public class GroupUserDetailDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }

    #endregion

    #region ===================== CREATE =====================

    public class CreateGroupDto
    {
        [Required(ErrorMessage = "Tên nhóm là bắt buộc.")]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public List<int>? UserIds { get; set; }
    }

    #endregion

    #region ===================== UPDATE =====================

    public class UpdateGroupDto
    {
        [Required(ErrorMessage = "Tên nhóm là bắt buộc.")]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public List<int>? UserIds { get; set; }
    }

    #endregion
}

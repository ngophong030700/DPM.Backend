using Shared.Application.BaseClass;
using System.ComponentModel.DataAnnotations;

namespace Shared.Application.DTOs.Identity
{
    #region ===================== VIEW LIST =====================

    public class ViewListUserDto
    {
        public int Id { get; set; }

        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public int? DepartmentId { get; set; }
        public string? Department { get; set; }

        public int? PositionId { get; set; }
        public string? Position { get; set; }

        public bool IsActive { get; set; }

        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public int? CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    #endregion

    #region ===================== VIEW DETAIL =====================

    public class ViewDetailUserDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public string? ImageUrl { get; set; }

        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int? DepartmentId { get; set; }
        public string? Department { get; set; }

        public int? PositionId { get; set; }
        public string? Position { get; set; }

        public bool IsActive { get; set; }

        public string? DistinguishedName { get; set; }
        public string? Sid { get; set; }

        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastSyncAt { get; set; }

        public int CreatedById { get; set; }
        public string? CreatedBy { get; set; }

        public int ModifiedById { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public List<UserGroupDto> Groups { get; set; } = new();
    }

    public class UserGroupDto
    {
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
    }

    public class UserLoginResultDto
    {
        public string Token { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
    #endregion

    #region ===================== CREATE =====================

    public class CreateUserDto
    {
        [Required(ErrorMessage = "Username là bắt buộc.")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "Password là bắt buộc.")]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "FullName là bắt buộc.")]
        public string FullName { get; set; } = default!;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress]
        public string Email { get; set; } = default!;

        public string? PhoneNumber { get; set; }

        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }

        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public string? DistinguishedName { get; set; }
        public string? Sid { get; set; }

        public List<int>? GroupIds { get; set; }
    }

    #endregion

    #region ===================== UPDATE =====================

    public class UpdateUserDto
    {
        public string? Password { get; set; }

        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }

        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }

        public string? ImageUrl { get; set; }

        public bool? IsActive { get; set; }

        public string? DistinguishedName { get; set; }
        public string? Sid { get; set; }

        public List<int>? GroupIds { get; set; }
    }

    #endregion

    #region ===================== FORM METADATA =====================

    public class UserFormMetadataDto
    {
        public List<BaseDto> Departments { get; set; } = new();
        public List<BaseDto> Positions { get; set; } = new();
        public List<BaseDto> Groups { get; set; } = new();
    }

    #endregion
}
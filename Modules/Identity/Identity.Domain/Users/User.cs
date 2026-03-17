using Identity.Domain.Departments;
using Identity.Domain.Groups;
using Identity.Domain.Positions;
using Shared.Domain;

namespace Identity.Domain.Users
{
    public class User : Entity, IAggregateRoot
    {
        private int _id;
        private string _username = string.Empty;
        private string _passwordHash = string.Empty;
        private string _fullName = string.Empty;
        private string _email = string.Empty;
        private string? _phoneNumber;
        private int? _departmentId;
        private Department? _department;
        private int? _positionId;
        private Position? _position;
        private string? _imageUrl;
        private bool _isActive;
        private string? _distinguishedName;
        private string? _sid;
        private DateTime? _lastLoginAt;
        private DateTime? _lastSyncAt;
        private DateTime _createdAt;
        private int _createdBy;
        private DateTime _modifiedAt;
        private int _modifiedBy;
        private bool _isDeleted;
        private bool? _gender;
        private DateTime? _dateOfBirth;
        private string? _address;

        private readonly List<UserGroup> _userGroups = new();
        public int Id => _id;
        public string Username => _username;
        public string PasswordHash => _passwordHash;
        public string FullName => _fullName;
        public string Email => _email;
        public string? PhoneNumber => _phoneNumber;
        public int? DepartmentId => _departmentId;
        public Department? Department => _department;
        public int? PositionId => _positionId;
        public Position? Position => _position;
        public string? ImageUrl => _imageUrl;
        public bool IsActive => _isActive;
        public string? DistinguishedName => _distinguishedName;
        public string? Sid => _sid;
        public DateTime? LastLoginAt => _lastLoginAt;
        public DateTime? LastSyncAt => _lastSyncAt;
        public DateTime CreatedAt => _createdAt;
        public int CreatedBy => _createdBy;
        public DateTime ModifiedAt => _modifiedAt;
        public int ModifiedBy => _modifiedBy;
        public bool IsDeleted => _isDeleted;
        public bool? Gender => _gender;
        public DateTime? DateOfBirth => _dateOfBirth;
        public string? Address => _address;

        public IReadOnlyCollection<UserGroup> UserGroups => _userGroups.AsReadOnly();

        private User() { }

        public static User Create(
            string username,
            string password,
            string fullName,
            string email,
            int createdBy,
            string? phoneNumber = null,
            bool? gender = null,
            DateTime? dateOfBirth = null,
            string? address = null,
            int? departmentId = null,
            int? positionId = null,
            string? imageUrl = null,
            bool isActive = true,
            string? distinguishedName = null,
            string? sid = null
        )
        {
            var user = new User
            {
                _username = username ?? throw new ArgumentNullException(nameof(username)),
                _passwordHash = HashPassword(password),
                _fullName = fullName ?? throw new ArgumentNullException(nameof(fullName)),
                _email = email ?? throw new ArgumentNullException(nameof(email)),
                _createdBy = createdBy,
                _modifiedBy = createdBy,
                _phoneNumber = phoneNumber,
                _gender = gender,
                _dateOfBirth = dateOfBirth,
                _address = address,
                _departmentId = departmentId,
                _positionId = positionId,
                _imageUrl = imageUrl,
                _isActive = isActive,
                _distinguishedName = distinguishedName,
                _sid = sid,
                _createdAt = DateTime.UtcNow,
                _modifiedAt = DateTime.UtcNow,
                _isDeleted = false
            };
            return user;
        }

        public void Update(
            string? password = null,
            string? fullName = null,
            string? email = null,
            int? modifiedBy = null,
            string? phoneNumber = null,
            bool? gender = null,
            DateTime? dateOfBirth = null,
            string? address = null,
            int? departmentId = null,
            int? positionId = null,
            string? imageUrl = null,
            bool? isActive = null,
            string? distinguishedName = null,
            string? sid = null)
        {
            if (!string.IsNullOrEmpty(password))
                _passwordHash = HashPassword(password);

            _fullName = fullName ?? _fullName;
            _email = email ?? _email;
            _phoneNumber = phoneNumber ?? _phoneNumber;
            _address = address ?? _address;
            _imageUrl = imageUrl ?? _imageUrl;
            _distinguishedName = distinguishedName ?? _distinguishedName;
            _sid = sid ?? _sid;

            _departmentId = departmentId ?? _departmentId;
            _positionId = positionId ?? _positionId;

            _gender = gender ?? _gender;
            _dateOfBirth = dateOfBirth ?? _dateOfBirth;

            _isActive = isActive ?? _isActive;

            _modifiedBy = modifiedBy ?? _modifiedBy;
            _modifiedAt = DateTime.UtcNow;
        }

        public void SetLastLoginAt(DateTime? lastLoginAt)
        {
            _lastLoginAt = lastLoginAt;
            _modifiedAt = DateTime.UtcNow;
        }

        public void SetLastSyncAt(DateTime lastSyncAt)
        {
            _lastSyncAt = lastSyncAt;
            _modifiedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            if (_isDeleted) return;
            _isDeleted = true;
            _modifiedAt = DateTime.UtcNow;
        }

        private static string HashPassword(string password)
        {
            string passwordHash = "";
            if (!string.IsNullOrEmpty(password))
                passwordHash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
            return passwordHash;
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, _passwordHash);
        }
    }
}
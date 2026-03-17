using Shared.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Shared.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId =>
            int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value, out var userId)
            ? userId
            : 0;

        public string UserName =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "";

        public string Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? "";

        public string FullName =>
            _httpContextAccessor.HttpContext?.User?.FindFirst("fullName")?.Value ?? UserName;

        /// <summary>
        /// Lấy token từ Authorization header (Bearer token)
        /// </summary>
        public string AccessToken
        {
            get
            {
                var authHeader = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return authHeader.Substring("Bearer ".Length).Trim();
                }
                return string.Empty;
            }
        }
    }
}

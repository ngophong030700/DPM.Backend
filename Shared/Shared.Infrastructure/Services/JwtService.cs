using Identity.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Infrastructure.Authentication
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["SecretKey"]
                ?? throw new Exception("Jwt SecretKey is missing");

            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim("fullName", user.FullName ?? ""),
                new Claim("email", user.Email ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
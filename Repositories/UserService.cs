using Microsoft.IdentityModel.Tokens;
using StarWarsAPI.Helpers;
using StarWarsAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StarWarsAPI.Repositories
{
    public class UserService : IUserService
    {
        private readonly List<AppUser> _users = new()
        {
            new AppUser { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") },
            new AppUser { Username = "user", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") }
        };

        public AppUser? Authenticate(string username, string password)
        {
            var user = _users.FirstOrDefault(x => x.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }
        public string GenerateJwtToken(string username, JwtSettings jwtSettings)
        {
            var role = string.Equals(username, UserRoles.Admin, StringComparison.OrdinalIgnoreCase)
                ? UserRoles.Admin
                : UserRoles.User;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                      DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                      ClaimValueTypes.Integer64
                )
            };  
                
            var     key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}

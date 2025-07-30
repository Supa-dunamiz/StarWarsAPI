using StarWarsAPI.Models;

namespace StarWarsAPI.Repositories
{
    public interface IUserService
    {
        AppUser? Authenticate(string username, string password);
        string GenerateJwtToken(string username, JwtSettings jwtSettings);
    }
}
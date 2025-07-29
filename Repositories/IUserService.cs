namespace StarWarsAPI.Repositories
{
    public interface IUserService
    {
        AppUser? Authenticate(string username, string password);
    }
}
namespace StarWarsAPI.Repositories
{
    public class UserService : IUserService
    {
        private readonly List<AppUser> _users = new()
    {
        new AppUser { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") },
        new AppUser { Username = "john", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") }
    };

        public AppUser? Authenticate(string username, string password)
        {
            var user = _users.FirstOrDefault(x => x.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }
    }
    public class AppUser
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Use hashed passwords
    }


}

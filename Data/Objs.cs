namespace StarWarsAPI.Models
{
    public class StarshipDto
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Cost_in_credits { get; set; }
        public string Length { get; set; }
        public string Max_atmosphering_speed { get; set; }
        public string Crew { get; set; }
        public string Passengers { get; set; }
        public string Cargo_capacity { get; set; }
        public string Consumables { get; set; }
        public string Hyperdrive_rating { get; set; }
        public string MGLT { get; set; }
        public string Starship_class { get; set; }
        public List<string> Films { get; set; }
        public List<string> Pilots { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
    }
    public class StarshipReadDTO : StarshipDto
    {
        public int Id { get; set; }
    }
    public class UpdateStarshipDto
    {
        public string? Name { get; set; }
        public string? Model { get; set; }
        public string? Manufacturer { get; set; }
        public string? Cost_in_credits { get; set; }
        public string? Length { get; set; }
        public string? Max_atmosphering_speed { get; set; }
        public string? Crew { get; set; }
        public string? Passengers { get; set; }
        public string? Cargo_capacity { get; set; }
        public string? Consumables { get; set; }
        public string? Hyperdrive_rating { get; set; }
        public string? MGLT { get; set; }
        public string? Starship_class { get; set; }
    }
    public class CreateStarshipDto
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Cost_in_credits { get; set; }
        public string Length { get; set; }
        public string Max_atmosphering_speed { get; set; }
        public string Crew { get; set; }
        public string Passengers { get; set; }
        public string Cargo_capacity { get; set; }
        public string Consumables { get; set; }
        public string Hyperdrive_rating { get; set; }
        public string MGLT { get; set; }
        public string Starship_class { get; set; }
        public string Url { get; set; }
        public List<int>? FilmIds { get; set; }
        public List<int>? PilotIds { get; set; }
    }
    public class AddFilmToStarshipDto
    {
        public int StarshipId { get; set; }
        public int FilmId { get; set; }
    }
    public class AddPilotToStarshipDto
    {
        public int StarshipId { get; set; }
        public int PilotId { get; set; }
    }
    public class StarshipQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "Name";
        public string? SortOrder { get; set; } = "asc"; // or "desc"
        public string? Search { get; set; }
    }
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpireMinutes { get; set; }
    }
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
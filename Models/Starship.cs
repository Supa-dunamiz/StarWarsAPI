namespace StarWarsAPI.Models
{
    public class Starship
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string CostInCredits { get; set; }
        public string Length { get; set; }
        public string MaxAtmospheringSpeed { get; set; }
        public string Crew { get; set; }
        public string Passengers { get; set; }
        public string CargoCapacity { get; set; }
        public string Consumables { get; set; }
        public string HyperdriveRating { get; set; }
        public string MGLT { get; set; }
        public string StarshipClass { get; set; }
        public ICollection<Film> Films { get; set; } = new List<Film>();
        public ICollection<Pilot> Pilots { get; set; } = new List<Pilot>();
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
    }
}

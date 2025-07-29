namespace StarWarsAPI.Models
{
    public class Pilot
    {
        public int Id { get; set; }
        public string Name { get; set; } = "N/A";
        public string Url { get; set; }

        public ICollection<Starship> Starships { get; set; } = new List<Starship>();
    }

}

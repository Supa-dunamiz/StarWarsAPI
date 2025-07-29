using Microsoft.EntityFrameworkCore;
using StarWarsAPI.Models;
using System.Text.Json;

namespace StarWarsAPI.Data
{
    public class Seed
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _http;

        public Seed(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _http = httpClient;
        }

        public async Task SeedStarshipsAsync()
        {
            if (await _context.Starships.AnyAsync())
                return;

            string url = "https://swapi.info/api/starships";

            var json = await _http.GetStringAsync(url);
            var response = JsonSerializer.Deserialize<List<StarshipDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (response == null || !response.Any())
                return;

            var processedFilmIds = new List<int>();
            var processedPilotIds = new List<int>();

            foreach (var dto in response)
            {
                var starship = new Starship
                {
                    Name = dto.Name,
                    Model = dto.Model,
                    Manufacturer = dto.Manufacturer,
                    CostInCredits = dto.Cost_in_credits,
                    Length = dto.Length,
                    MaxAtmospheringSpeed = dto.Max_atmosphering_speed,
                    Crew = dto.Crew,
                    Passengers = dto.Passengers,
                    CargoCapacity = dto.Cargo_capacity,
                    Consumables = dto.Consumables,
                    HyperdriveRating = dto.Hyperdrive_rating,
                    MGLT = dto.MGLT,
                    StarshipClass = dto.Starship_class,
                    Url = dto.Url,
                    Created = dto.Created,
                    Edited = dto.Edited,
                };

                // Handle Films
                foreach (var filmUrl in dto.Films)
                {
                    var filmId = ExtractIdFromUrl(filmUrl);

                    if (filmId > 0 && !processedFilmIds.Contains(filmId))
                    {
                        var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == filmId);

                        if (film == null)
                        {
                            film = new Film
                            {
                                Id = filmId,
                                Url = filmUrl
                            };
                            _context.Films.Add(film);
                        }

                        processedFilmIds.Add(filmId);
                        starship.Films.Add(film);
                    }
                }

                // Handle Pilots
                foreach (var pilotUrl in dto.Pilots)
                {
                    var pilotId = ExtractIdFromUrl(pilotUrl);

                    if (pilotId > 0 && !processedPilotIds.Contains(pilotId))
                    {
                        var pilot = await _context.Pilots.FirstOrDefaultAsync(p => p.Id == pilotId);

                        if (pilot == null)
                        {
                            pilot = new Pilot
                            {
                                Id = pilotId,
                                Url = pilotUrl
                            };
                            _context.Pilots.Add(pilot);
                        }

                        processedPilotIds.Add(pilotId);
                        starship.Pilots.Add(pilot);
                    }
                }


                _context.Starships.Add(starship);
            }

            await _context.SaveChangesAsync();

        }


        private int ExtractIdFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return -1;

            var parts = url.TrimEnd('/').Split('/');
            return int.TryParse(parts.Last(), out var id) ? id : -1;
        }
    }

}

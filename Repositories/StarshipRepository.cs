using Microsoft.EntityFrameworkCore;
using StarWarsAPI.Data;
using StarWarsAPI.Helpers;
using StarWarsAPI.Models;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StarWarsAPI.Repositories
{
    public class StarshipRepository : IStarshipRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StarshipRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<StarshipDto>> GetAllAsync()
        {
            return await _context.Starships
                .Include(s => s.Films)
                .Include(s => s.Pilots)
                .Select(s => new StarshipDto
                {
                    Name = s.Name,
                    Model = s.Model,
                    Manufacturer = s.Manufacturer,
                    Cost_in_credits = s.CostInCredits,
                    Length = s.Length,
                    Max_atmosphering_speed = s.MaxAtmospheringSpeed,
                    Crew = s.Crew,
                    Passengers = s.Passengers,
                    Cargo_capacity = s.CargoCapacity,
                    Consumables = s.Consumables,
                    Hyperdrive_rating = s.HyperdriveRating,
                    MGLT = s.MGLT,
                    Starship_class = s.StarshipClass,
                    Created = s.Created,
                    Edited = s.Edited,
                    Url = UrlBuilder.BuildStarshipUrl(_httpContextAccessor.HttpContext, s.Id),
                    Films = s.Films.Select(f => f.Url).ToList(),
                    Pilots = s.Pilots.Select(p => p.Url).ToList()
                })
                .ToListAsync();
        }
        public async Task<StarshipDto?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;

            var s = await _context.Starships
                .Include(s => s.Films)
                .Include(s => s.Pilots)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (s == null) return null;

            return new StarshipDto
            {
                Name = s.Name,
                Model = s.Model,
                Manufacturer = s.Manufacturer,
                Cost_in_credits = s.CostInCredits,
                Length = s.Length,
                Max_atmosphering_speed = s.MaxAtmospheringSpeed,
                Crew = s.Crew,
                Passengers = s.Passengers,
                Cargo_capacity = s.CargoCapacity,
                Consumables = s.Consumables,
                Hyperdrive_rating = s.HyperdriveRating,
                MGLT = s.MGLT,
                Starship_class = s.StarshipClass,
                Created = s.Created,
                Edited = s.Edited,
                Url = UrlBuilder.BuildStarshipUrl(_httpContextAccessor.HttpContext, s.Id),
                Films = s.Films.Select(f => f.Url).ToList(),
                Pilots = s.Pilots.Select(p => p.Url).ToList()
            };
        }
        public async Task<PagedResult<StarshipReadDTO>> GetPagedAsync(StarshipQueryParameters query)
        {
            var rawData = await _context.Starships
                .Include(s => s.Films)
                .Include(s => s.Pilots)
                .AsNoTracking()
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                rawData = rawData.Where(s =>
                    (!string.IsNullOrWhiteSpace(s.Name) && s.Name.Contains(query.Search, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(s.Model) && s.Model.Contains(query.Search, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(s.StarshipClass) && s.StarshipClass.Contains(query.Search, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            var sortedData = StarshipHelper.ApplySorting(rawData, query.SortBy, query.SortOrder);

            var pagedItems = sortedData
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(s => CreateDto(s))
                .ToList();

            return new PagedResult<StarshipReadDTO>
            {
                Items = pagedItems,
                TotalCount = rawData.Count,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
        public async Task<bool> CreateAsync(CreateStarshipDto dto)
        {
            if (dto == null) return false;

            var starship = StarshipHelper.MapToStarship(dto);

            if (dto.FilmIds != null && dto.FilmIds.Any())
            {
                var films = await _context.Films
                    .Where(f => dto.FilmIds.Contains(f.Id))
                    .ToListAsync();
                starship.Films = films;
            }

            if (dto.PilotIds != null && dto.PilotIds.Any())
            {
                var pilots = await _context.Pilots
                    .Where(p => dto.PilotIds.Contains(p.Id))
                    .ToListAsync();
                starship.Pilots = pilots;
            }

            _context.Starships.Add(starship);

            return await SaveAsync();
        }
        public async Task<bool> UpdateAsync(int id, UpdateStarshipDto dto)
        {
            if (dto == null || id <= 0) return false;

            var existing = await _context.Starships.FindAsync(id);
            if (existing == null) return false;

            StarshipHelper.UpdateStarshipFromDto(existing, dto);

            return await SaveAsync();
        }
        public async Task<bool> AddFilmToStarshipAsync(int starshipId, int filmId)
        {
            if (filmId <= 0 || starshipId <= 0) return false;

            var ship = await _context.Starships.Include(s => s.Films).FirstOrDefaultAsync(s => s.Id == starshipId);
            if (ship == null) return false;

            var film = await _context.Films.FindAsync(filmId);
            if (film == null) return false;

            if (!ship.Films.Contains(film))
            {
                ship.Films.Add(film);
                return await SaveAsync();
            }

            return true;
        }
        public async Task<bool> AddPilotToStarshipAsync(int starshipId, int pilotId)
        {
            if (pilotId <= 0 || starshipId <= 0) return false;

            var ship = await _context.Starships.Include(s => s.Pilots).FirstOrDefaultAsync(s => s.Id == starshipId);
            if (ship == null) return false;

            var pilot = await _context.Pilots.FindAsync(pilotId);
            if (pilot == null) return false;

            if (!ship.Pilots.Contains(pilot))
            {
                ship.Pilots.Add(pilot);
                return await SaveAsync();
            }

            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) return false;

            var ship = await _context.Starships.FindAsync(id);
            if (ship == null) return false;

            _context.Starships.Remove(ship);
            return await SaveAsync();
        }

        private async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        private StarshipReadDTO CreateDto(Starship s)
        {
            return new StarshipReadDTO
            {
                Id = s.Id,
                Name = s.Name,
                Model = s.Model,
                Manufacturer = s.Manufacturer,
                Cost_in_credits = s.CostInCredits,
                Length = s.Length,
                Max_atmosphering_speed = s.MaxAtmospheringSpeed,
                Crew = s.Crew,
                Passengers = s.Passengers,
                Cargo_capacity = s.CargoCapacity,
                Consumables = s.Consumables,
                Hyperdrive_rating = s.HyperdriveRating,
                MGLT = s.MGLT,
                Starship_class = s.StarshipClass,
                Url = UrlBuilder.BuildStarshipUrl(_httpContextAccessor.HttpContext, s.Id),
                Created = s.Created,
                Edited = s.Edited,
                Films = s.Films?.Select(f => f.Url).ToList() ?? new(),
                Pilots = s.Pilots?.Select(p => p.Url).ToList() ?? new()
            };
        }

        private static class StarshipHelper
        {
            public static void UpdateStarshipFromDto(Starship existing, UpdateStarshipDto dto)
            {
                if(existing == null ||  dto == null) return;

                existing.Edited = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(dto.Name))
                    existing.Name = dto.Name;

                if (!string.IsNullOrWhiteSpace(dto.Model))
                    existing.Model = dto.Model;

                if (!string.IsNullOrWhiteSpace(dto.Manufacturer))
                    existing.Manufacturer = dto.Manufacturer;

                if (!string.IsNullOrWhiteSpace(dto.Cost_in_credits))
                    existing.CostInCredits = dto.Cost_in_credits;

                if (!string.IsNullOrWhiteSpace(dto.Length))
                    existing.Length = dto.Length;

                if (!string.IsNullOrWhiteSpace(dto.Max_atmosphering_speed))
                    existing.MaxAtmospheringSpeed = dto.Max_atmosphering_speed;

                if (!string.IsNullOrWhiteSpace(dto.Crew))
                    existing.Crew = dto.Crew;

                if (!string.IsNullOrWhiteSpace(dto.Passengers))
                    existing.Passengers = dto.Passengers;

                if (!string.IsNullOrWhiteSpace(dto.Cargo_capacity))
                    existing.CargoCapacity = dto.Cargo_capacity;

                if (!string.IsNullOrWhiteSpace(dto.Consumables))
                    existing.Consumables = dto.Consumables;

                if (!string.IsNullOrWhiteSpace(dto.Hyperdrive_rating))
                    existing.HyperdriveRating = dto.Hyperdrive_rating;

                if (!string.IsNullOrWhiteSpace(dto.MGLT))
                    existing.MGLT = dto.MGLT;

                if (!string.IsNullOrWhiteSpace(dto.Starship_class))
                    existing.StarshipClass = dto.Starship_class;
            }
            public static Starship MapToStarship(CreateStarshipDto dto)
            {
                return new Starship
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
                    Url = dto.Url
                };
            }
            public static IEnumerable<Starship> ApplySorting(IEnumerable<Starship> query, string? sortBy, string? sortOrder)
            {
                sortBy = sortBy?.ToLowerInvariant() ?? "name";
                bool desc = sortOrder?.ToLowerInvariant() == "desc";

                return sortBy switch
                {
                    "name" => desc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                    "model" => desc ? query.OrderByDescending(s => s.Model) : query.OrderBy(s => s.Model),
                    "cost_in_credits" => desc
                        ? query.OrderByDescending(s => ParseDecimal(s.CostInCredits))
                        : query.OrderBy(s => ParseDecimal(s.CostInCredits)),
                    "length" => desc
                        ? query.OrderByDescending(s => ParseDecimal(s.Length))
                        : query.OrderBy(s => ParseDecimal(s.Length)),
                    "max_atmosphering_speed" => desc
                        ? query.OrderByDescending(s => ParseDecimal(s.MaxAtmospheringSpeed))
                        : query.OrderBy(s => ParseDecimal(s.MaxAtmospheringSpeed)),
                    "passengers" => desc
                        ? query.OrderByDescending(s => ParseInt(s.Passengers))
                        : query.OrderBy(s => ParseInt(s.Passengers)),
                    "cargo_capacity" => desc
                        ? query.OrderByDescending(s => ParseDecimal(s.CargoCapacity))
                        : query.OrderBy(s => ParseDecimal(s.CargoCapacity)),
                    "hyperdrive_rating" => desc
                        ? query.OrderByDescending(s => ParseDecimal(s.HyperdriveRating))
                        : query.OrderBy(s => ParseDecimal(s.HyperdriveRating)),
                    _ => query.OrderBy(s => s.Name)
                };
            }
            private static decimal ParseDecimal(string? value)
            {
                if (string.IsNullOrWhiteSpace(value) || value.Trim().ToLower() == "unknown") return 0;
                return decimal.TryParse(value.Replace(",", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                    ? result
                    : 0;
            }
            private static int ParseInt(string? value)
            {
                if (string.IsNullOrWhiteSpace(value) || value.Trim().ToLower() == "unknown") return 0;
                return int.TryParse(value.Replace(",", ""), out var result) ? result : 0;
            }
        }
    }

}

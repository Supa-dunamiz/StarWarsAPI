using StarWarsAPI.Models;

namespace StarWarsAPI.Repositories
{
    public interface IStarshipRepository
    {
        Task<bool> AddFilmToStarshipAsync(int starshipId, int filmId);
        Task<bool> AddPilotToStarshipAsync(int starshipId, int pilotId);
        Task<bool> RemovePilotFromStarshipAsync(int starshipId, int pilotId);
        Task<bool> RemoveFilmFromStarshipAsync(int starshipId, int filmId);
        Task<bool> CreateAsync(CreateStarshipDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<StarshipDto>> GetAllAsync();
        Task<StarshipDto?> GetByIdAsync(int id);
        Task<PagedResult<StarshipReadDTO>> GetPagedAsync(StarshipQueryParameters query);
        Task<bool> UpdateAsync(int id, UpdateStarshipDto dto);
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarWarsAPI.Helpers;
using StarWarsAPI.Models;
using StarWarsAPI.Repositories;

namespace StarWarsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StarshipsController : ControllerBase
    {
        private readonly IStarshipRepository _repo;

        public StarshipsController(IStarshipRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetStarShips()
        {
            var ships = await _repo.GetAllAsync();
            return Ok(ships);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStarShipById(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpGet("paged")]
        public async Task<IActionResult> GetStarShipsPaged([FromQuery] StarshipQueryParameters query)
        {
            var result = await _repo.GetPagedAsync(query);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateStarShip([FromBody] CreateStarshipDto starship)
        {
            var done = await _repo.CreateAsync(starship);
            return Ok(done);
        }

        [HttpPost("{starshipId}/films/{filmId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddFilm(int starshipId, int filmId)
        {
            var success = await _repo.AddFilmToStarshipAsync(starshipId, filmId);
            return Ok(success);
        }

        [HttpPost("{starshipId}/pilots/{pilotId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddPilot(int starshipId, int pilotId)
        {
            var success = await _repo.AddPilotToStarshipAsync(starshipId, pilotId);
            return Ok(success);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStarshipDto dto)
        {
            if (dto == null)
                return BadRequest("Update data is required.");

            var updated = await _repo.UpdateAsync(id, dto);
            return updated ? Ok(true) : NotFound($"Starship with ID {id} not found.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var done = await _repo.DeleteAsync(id);
            return Ok(done);
        }
    }
}

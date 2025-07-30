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
    public class StarshipController : ControllerBase
    {
        private readonly IStarshipRepository _repo;

        public StarshipController(IStarshipRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        [HttpGet("Ships")]
        public async Task<IActionResult> GetAll()
        {
            var ships = await _repo.GetAllAsync();
            return Ok(ships);
        }

        [Authorize]
        [HttpGet("Ships/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpGet("GetPagedShips")]
        public async Task<IActionResult> GetPaged([FromQuery] StarshipQueryParameters query)
        {
            var result = await _repo.GetPagedAsync(query);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("CreateShip")]
        public async Task<IActionResult> Create([FromBody] CreateStarshipDto starship)
        {
            var done = await _repo.CreateAsync(starship);
            return Ok(done);
        }

        [HttpPost("AddFilm")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddFilm([FromBody] AddFilmToStarshipDto dto)
        {
            var success = await _repo.AddFilmToStarshipAsync(dto.StarshipId, dto.FilmId);
            return Ok(success);
        }

        [HttpPost("AddPilot")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddPilot([FromBody] AddPilotToStarshipDto dto)
        {
            var success = await _repo.AddPilotToStarshipAsync(dto.StarshipId, dto.PilotId);
            return Ok(success);
        }

        [HttpPut("UpdateShip/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStarshipDto dto)
        {
            if (dto == null)
                return BadRequest("Update data is required.");

            var updated = await _repo.UpdateAsync(id, dto);
            if (!updated)
                return NotFound($"Starship with ID {id} not found.");

            return Ok(true);
        }

        [HttpDelete("DeleteShip/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            bool done = await _repo.DeleteAsync(id);
            return Ok(done);
        }
    }
}

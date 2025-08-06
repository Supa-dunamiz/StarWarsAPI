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

        //[HttpGet()]
        //public async Task<IActionResult> GetStarShips()
        //{
        //    var ships = await _repo.GetAllAsync();
        //    return Ok(ships);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStarShipById(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetStarShipsPaged([FromQuery] StarshipQueryParameters query)
        {
            var result = await _repo.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStarShip([FromBody] CreateStarshipDto starship)
        {
            var done = await _repo.CreateAsync(starship);
            return Ok(done);
        }

        [HttpPost("AddFilmToStarship")]
        [Authorize(Roles = UserRoles.Admin)]  
        public async Task<IActionResult> AddFilm([FromBody] UpdateFilmDTO dto)
        {
            var success = await _repo.AddFilmToStarshipAsync(dto.StarshipId, dto.FilmId);
            return Ok(success);
        }

        [HttpPost("AddPilotToStarship")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddPilot([FromBody] UpdatePilotDTO dto)
        {
            var success = await _repo.AddPilotToStarshipAsync(dto.StarshipId, dto.PilotId);
            return Ok(success);
        }

        [HttpPut("RemovePilotFromStarship")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RemovePilot([FromBody] UpdatePilotDTO dto)
        {
            var success = await _repo.RemovePilotFromStarshipAsync(dto.StarshipId, dto.PilotId);
            return Ok(success);
        }
        [HttpPut("RemoveFilmFromStarship")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RemoveFilm([FromBody] UpdateFilmDTO dto)
        {
            var success = await _repo.RemoveFilmFromStarshipAsync(dto.StarshipId, dto.FilmId);
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

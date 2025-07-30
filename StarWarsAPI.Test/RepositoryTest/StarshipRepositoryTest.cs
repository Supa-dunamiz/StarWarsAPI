using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Xunit;
using StarWarsAPI.Data;
using StarWarsAPI.Models;
using StarWarsAPI.Repositories;
using System;

public class StarshipRepositoryTest
{
    private readonly AppDbContext _context;
    private readonly StarshipRepository _repository;

    public StarshipRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StarshipTestDb")
            .Options;

        _context = new AppDbContext(options);

        // Seed data
        SeedTestData(_context);

        var httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext()
        };
        httpContextAccessor.HttpContext.Request.Scheme = "https";
        httpContextAccessor.HttpContext.Request.Host = new HostString("localhost", 5001);

        _repository = new StarshipRepository(_context, httpContextAccessor);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStarships()
    {
        //Arrange
        IEnumerable<StarshipDto> result;
        
        // Act
        result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count());

        var starship = result.First();
        Assert.Equal("Millennium Falcon", starship.Name);
        Assert.Single(starship.Films);
        Assert.Single(starship.Pilots);
    }
    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectStarship()
    {
        //Arrange
        StarshipDto result = new StarshipDto();

        // Act
        result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Millennium Falcon", result.Name);
        Assert.Equal("YT-1300", result.Model);
        Assert.Single(result.Films);
        Assert.Single(result.Pilots);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ShouldReturnNull()
    {
        var result = await _repository.GetByIdAsync(999);
        Assert.Null(result);
    }
    private void SeedTestData(AppDbContext context)
    {
        var film = new Film { Id = 1, Url = "https://swapi.dev/api/films/1/" };
        var pilot = new Pilot { Id = 1, Url = "https://swapi.dev/api/people/1/" };

        var starship = new Starship
        {
            Id = 1,
            Name = "Millennium Falcon",
            Model = "YT-1300",
            Manufacturer = "Corellian Engineering Corporation",
            CostInCredits = "100000",
            Length = "34.75",
            MaxAtmospheringSpeed = "1050",
            Crew = "4",
            Passengers = "6",
            CargoCapacity = "100000",
            Consumables = "2 months",
            HyperdriveRating = "0.5",
            MGLT = "75",
            StarshipClass = "Light freighter",
            Created = System.DateTime.UtcNow,
            Edited = System.DateTime.UtcNow,
            Url = "https://swapi.dev/api/starships/10/",
            Films = new List<Film> { film },
            Pilots = new List<Pilot> { pilot }
        };

        context.Starships.Add(starship);
        context.SaveChanges();
    }
}

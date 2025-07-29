using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System;

namespace StarWarsAPI.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; } = "N/A";
        public string Url { get; set; }
        public ICollection<Starship> Starships { get; set; } = new List<Starship>();

    }

}

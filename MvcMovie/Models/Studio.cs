using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models;

public class Studio
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Country { get; set; }
    public string Site { get; set; }
    public ICollection<Movie>? Movies { get; set; }
    // public ICollection<Movie>? Movies { get; } = new List<Movie>();
}

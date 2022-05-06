using System.ComponentModel.DataAnnotations;

namespace GeoComment.Models;

public class Comment
{
    public int Id { get; set; }
    public string Title { get; set; }
    [Required] public string Message { get; set; }
    [Required] public double Longitude { get; set; }
    [Required] public double Latitude { get; set; }
    public string AuthorName { get; set; }
    public GeoUser Author { get; set; }
}
namespace GeoComment.Models;

public class Comment
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string AuthorName { get; set; }
    public GeoUser Author { get; set; }
}
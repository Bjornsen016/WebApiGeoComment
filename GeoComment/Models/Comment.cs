namespace GeoComment.Models;

public class Comment
{
    public int Id { get; set; }
    public string Author { get; set; }
    public string Message { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}
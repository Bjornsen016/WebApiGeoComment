namespace GeoComment.DTOs;

public class CommentInputDTO
{
    public string Author { get; set; }
    public string Message { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}
namespace GeoComment.DTOs;

public class CommentInputDTO
{
    /// <example>Mark</example>
    public string Author { get; set; }

    /// <example>Lorem ipsum dolor amet</example>
    public string Message { get; set; }

    /// <example>5</example>
    public double Longitude { get; set; }

    /// <example>5</example>
    public double Latitude { get; set; }
}
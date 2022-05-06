namespace GeoComment.DTOs;

public class CommentReturnDTO
{
    /// <example>1</example>
    public int Id { get; set; }

    /// <example>Mark</example>
    public string Author { get; set; }

    /// <example>Lorem ipsum dolor amet</example>
    public string Message { get; set; }

    /// <example>5</example>
    public double Longitude { get; set; }

    /// <example>5</example>
    public double Latitude { get; set; }
}
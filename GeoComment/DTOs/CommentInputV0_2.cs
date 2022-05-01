namespace GeoComment.DTOs;

public class CommentInputV0_2
{
    public InputBody Body { get; set; }

    /// <example>5</example>
    public double Longitude { get; set; }

    /// <example>5</example>
    public double Latitude { get; set; }
}

public class InputBody
{
    /// <example>Lorem</example>
    public string Title { get; set; }

    /// <example>Lorem ipsum dolor amet</example>
    public string Message { get; set; }
}
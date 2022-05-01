namespace GeoComment.DTOs;

public class CommentInputV0_2
{
    public InputBody Body { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}

public class InputBody
{
    public string Title { get; set; }
    public string Message { get; set; }
}
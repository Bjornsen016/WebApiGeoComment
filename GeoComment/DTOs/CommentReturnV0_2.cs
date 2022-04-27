namespace GeoComment.DTOs;

public class CommentReturnV0_2
{
    public int Id { get; set; }
    public CommentReturnBody Body { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}

public class CommentReturnBody
{
    public string Title => Message.Split(" ")[0];
    public string Author { get; set; }
    public string Message { get; set; }
}
namespace GeoComment.DTOs;

public class CommentReturnV0_2
{
    public static CommentReturnV0_2 CreateReturn(Comment comment)
    {
        var commentReturn = new CommentReturnV0_2
        {
            Body = new CommentReturnBody
            {
                Author = comment.AuthorName,
                Message = comment.Message,
                Title = comment.Title
            },
            Id = comment.Id,
            Latitude = comment.Latitude,
            Longitude = comment.Longitude
        };
        return commentReturn;
    }

    /// <example>1</example>
    public int Id { get; set; }

    public CommentReturnBody Body { get; set; }

    /// <example>5</example>
    public double Longitude { get; set; }

    /// <example>5</example>
    public double Latitude { get; set; }
}

public class CommentReturnBody
{
    /// <example>Lorem</example>
    public string Title { get; set; }

    /// <example>Mark</example>
    public string Author { get; set; }

    /// <example>Lorem ipsum dolor amet</example>
    public string Message { get; set; }
}
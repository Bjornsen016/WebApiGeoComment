namespace GeoComment.Models;

public class GeoUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Comment> Comments { get; set; }
}
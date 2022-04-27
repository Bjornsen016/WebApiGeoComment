using Microsoft.AspNetCore.Identity;

namespace GeoComment.Models;

public class GeoUser : IdentityUser
{
    public string Name { get; set; }
    public List<Comment> Comments { get; set; }
}
using Microsoft.AspNetCore.Identity;

namespace GeoComment.Models;

public class GeoUser : IdentityUser
{
    public List<Comment> Comments { get; set; }
}
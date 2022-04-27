using System.Linq.Expressions;

namespace GeoComment.Services;

public class Database
{
    private readonly GeoDbContext _geoDbContext;

    public Database(GeoDbContext geoDbContext)
    {
        _geoDbContext = geoDbContext;
    }

    public async Task RecreateDb()
    {
        await _geoDbContext.Database.EnsureDeletedAsync();
        await _geoDbContext.Database.EnsureCreatedAsync();
        await SeedUsers();
    }

    public async Task SeedUsers()
    {
        var users = new GeoUser[]
        {
            new() {UserName = "Ada"},
            new() {UserName = "Bill"}
        };

        await _geoDbContext.Authors.AddRangeAsync(users);
        await _geoDbContext.SaveChangesAsync();
    }
}
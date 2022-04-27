using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;

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
    }
}
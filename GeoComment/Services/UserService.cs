using GeoComment.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GeoComment.Services;

public class UserService
{
    private readonly UserManager<GeoUser> _userManager;

    public UserService(UserManager<GeoUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<GeoUser> RegisterNewUser(RegisterUserInput input)
    {
        var user = new GeoUser
        {
            UserName = input.UserName
        };
        var result = await _userManager.CreateAsync(user, input.Password);
        return result.Succeeded ? await _userManager.FindByNameAsync(input.UserName) : null;
    }
}
using GeoComment.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GeoComment.Services;

public class UserService
{
    private readonly UserManager<GeoUser> _userManager;
    private readonly JwtManager _jwtManager;

    public UserService(UserManager<GeoUser> userManager, JwtManager jwtManager)
    {
        _userManager = userManager;
        _jwtManager = jwtManager;
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

    public async Task<string> Login(RegisterUserInput input)
    {
        var user = await _userManager.FindByNameAsync(input.UserName);
        if (user is null) return null;
        var result = await _userManager.CheckPasswordAsync(user, input.Password);

        if (!result) return null;

        string token = _jwtManager.GenerateJwtToken(user);
        return token;
    }

    public async Task<GeoUser> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return user;
    }
}
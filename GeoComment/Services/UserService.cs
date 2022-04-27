using GeoComment.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GeoComment.Services;

public class UserService
{
    private readonly UserManager<GeoUser> _userManager;
    private readonly SignInManager<GeoUser> _signInManager;

    public UserService(UserManager<GeoUser> userManager, SignInManager<GeoUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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

    public async Task Login(RegisterUserInput input)
    {
        var user = await _userManager.FindByNameAsync(input.UserName);
        var result = await _signInManager.PasswordSignInAsync(user, input.Password, false, false);

        if (result.Succeeded)
        {
            //TODO: Generate JWT token and return it.
        }
    }
}
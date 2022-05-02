using GeoComment.DTOs;
using GeoComment.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers;

[Route("api/user")]
[ApiVersion("0.2")]
[ControllerName("User Controller")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("register")]
    public async Task<ActionResult<RegisterUserReturn>> RegisterNewUser(RegisterUserInput input)
    {
        GeoUser newUser = await _userService.RegisterNewUser(input);

        if (newUser == null) return BadRequest();

        RegisterUserReturn returnUser = new RegisterUserReturn
        {
            Id = newUser.Id,
            Username = newUser.UserName
        };
        return CreatedAtAction(nameof(RegisterNewUser), returnUser);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("login")]
    public async Task<ActionResult> Login(RegisterUserInput input)
    {
        string token = await _userService.Login(input);

        if (token is null) return BadRequest();

        var json = new {token = token};

        return Ok(json);
    }
}
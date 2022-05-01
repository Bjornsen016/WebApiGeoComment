using System.ComponentModel;
using System.Security.Claims;
using AutoMapper;
using GeoComment.DTOs;
using GeoComment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Controllers;

[Route("api/geo-comments")]
[ControllerName("Geo Controller")]
[ApiVersion("0.2")]
[ApiController]
public class GeoControllerV0_2 : ControllerBase
{
    private readonly GeoCommentService _geoCommentService;
    private readonly UserService _userService;

    public GeoControllerV0_2(GeoCommentService geoCommentService, UserService userService)
    {
        _geoCommentService = geoCommentService;
        _userService = userService;
    }

    /// <summary>
    /// Gets a specific comment by id
    /// </summary>
    /// <param name="id">Id of the comment</param>
    /// <returns>The comment</returns>
    /// <response code="200">Success: Returns the comment</response>
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [HttpGet]
    public async Task<ActionResult<CommentReturnV0_2>> Get(int id)
    {
        var comment = await _geoCommentService.GetCommentById(id);

        if (comment == null) return NotFound();

        var commentReturn = CommentReturnV0_2.CreateReturn(comment);

        return Ok(commentReturn);
    }

    /// <summary>
    /// Creates a comment
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST api/geo-comments
    ///     {
    ///         "body": {
    ///             "title": "Lorem",
    ///             "message": "Lorem ipsum dolor amet"
    ///         },
    ///         "longitude": 5,
    ///         "latitude": 5
    ///     }
    /// </remarks>
    /// <param name="input">Comment input</param>
    /// <returns>The comment if created</returns>
    /// <response code="201">Returns the created comment</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<ActionResult<CommentReturnV0_2>> CreateComment(CommentInputV0_2 input)
    {
        var userPrincipal = HttpContext.User;

        var id = userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var user = await _userService.GetUser(id);

        if (user is null) return Unauthorized();

        try
        {
            var comment = new Comment
            {
                Author = user,
                AuthorName = user.UserName,
                Latitude = input.Latitude,
                Longitude = input.Longitude,
                Message = input.Body.Message,
                Title = input.Body.Title
            };
            var createdComment = await _geoCommentService.CreateCommentInDb(comment);
            var commentReturn = CommentReturnV0_2.CreateReturn(createdComment);

            return CreatedAtAction(nameof(Get), new {id = commentReturn.Id}, commentReturn);
        }
        catch (DbUpdateException)
        {
            return NotFound("A database error occurred");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///     
    /// </remarks>
    /// <param name="username">Username of the Author</param>
    /// <returns>The users comments</returns>
    /// <response code="200">Success: Returns all the comments by the user</response>
    [HttpGet]
    [Route("{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<ActionResult<List<CommentReturnV0_2>>> GetUserComments(string username)
    {
        var comments = await _geoCommentService.GetCommentsByUser(username);

        if (comments.Count == 0) return NotFound();

        List<CommentReturnV0_2> returnComments = new List<CommentReturnV0_2>();

        foreach (var comment in comments)
        {
            var cmt = CommentReturnV0_2.CreateReturn(comment);

            returnComments.Add(cmt);
        }

        return Ok(returnComments);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="minLon"></param>
    /// <param name="maxLon"></param>
    /// <param name="minLat"></param>
    /// <param name="maxLat"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<ActionResult<List<CommentReturnV0_2>>> GetComments(
        [BindRequired] double minLon, [BindRequired] double maxLon,
        [BindRequired] double minLat, [BindRequired] double maxLat)
    {
        var comments = await _geoCommentService.GetComments(minLon, maxLon, minLat, maxLat);

        var returnComments = new List<CommentReturnV0_2>();
        foreach (var comment in comments)
        {
            var cmt = CommentReturnV0_2.CreateReturn(comment);

            returnComments.Add(cmt);
        }

        return Ok(returnComments);
    }

    /// <summary>
    /// Deletes the comment
    /// </summary>
    /// <param name="id">Id of the comment to be deleted</param>
    /// <returns>The deleted comment</returns>
    [HttpDelete]
    [Authorize]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<ActionResult<CommentReturnV0_2>> DeleteComment(int id)
    {
        var userPrincipal = HttpContext.User;

        var userId = userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

        try
        {
            var deletedComment = await _geoCommentService.DeleteCommentById(id, userId);
            if (deletedComment is null) return NotFound();

            var returnComment = CommentReturnV0_2.CreateReturn(deletedComment);

            return Ok(returnComment);
        }
        catch (UnauthorizedException)
        {
            return Unauthorized();
        }
    }
}
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
    private readonly IMapper _mapper;
    private readonly UserService _userService;

    public GeoControllerV0_2(GeoCommentService geoCommentService, IMapper mapper, UserService userService)
    {
        _geoCommentService = geoCommentService;
        _mapper = mapper;
        _userService = userService;
    }

    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<CommentReturnV0_2>> Get(int id)
    {
        var comment = await _geoCommentService.GetCommentById(id);

        if (comment == null) return NotFound();

        var commentReturn = new CommentReturnV0_2
        {
            Body = new CommentReturnBody
            {
                Author = comment.AuthorName,
                Message = comment.Message,
                Title = comment.Title
            },
            Id = comment.Id,
            Latitude = comment.Latitude,
            Longitude = comment.Longitude
        };

        return Ok(commentReturn);
    }


    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
            var commentReturn = new CommentReturnV0_2
            {
                Body = new CommentReturnBody
                {
                    Author = createdComment.AuthorName,
                    Message = createdComment.Message,
                    Title = createdComment.Title
                },
                Id = createdComment.Id,
                Latitude = createdComment.Latitude,
                Longitude = createdComment.Longitude
            };

            return CreatedAtAction(nameof(Get), new {id = commentReturn.Id}, commentReturn);
        }
        catch (DbUpdateException)
        {
            return NotFound("A database error occurred");
        }
    }

    [HttpGet]
    [Route("{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CommentReturnV0_2>>> GetUserComments(string username)
    {
        var comments = await _geoCommentService.GetCommentsByUser(username);

        if (comments.Count == 0) return NotFound();

        List<CommentReturnV0_2> returnComments = new List<CommentReturnV0_2>();

        foreach (var comment in comments)
        {
            var cmt = new CommentReturnV0_2
            {
                Body = new CommentReturnBody
                {
                    Author = comment.AuthorName,
                    Message = comment.Message,
                    Title = comment.Title
                },
                Id = comment.Id,
                Latitude = comment.Latitude,
                Longitude = comment.Longitude
            };

            returnComments.Add(cmt);
        }

        return Ok(returnComments);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CommentReturnV0_2>>> GetComments(
        [BindRequired] double minLon, [BindRequired] double maxLon,
        [BindRequired] double minLat, [BindRequired] double maxLat)
    {
        var comments = await _geoCommentService.GetComments(minLon, maxLon, minLat, maxLat);

        var returnComments = new List<CommentReturnV0_2>();
        foreach (var comment in comments)
        {
            var cmt = new CommentReturnV0_2
            {
                Body = new CommentReturnBody
                {
                    Author = comment.AuthorName,
                    Message = comment.Message,
                    Title = comment.Title
                },
                Id = comment.Id,
                Latitude = comment.Latitude,
                Longitude = comment.Longitude
            };

            returnComments.Add(cmt);
        }

        return Ok(returnComments);
    }

    [HttpDelete]
    [Authorize]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommentReturnV0_2>> DeleteComment(int id)
    {
        //TODO: Implement delete so the correct user can delete his/her comment and not someone else. Also can't delete a comment that does not exist ofc.
        var userPrincipal = HttpContext.User;

        var userId = userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

        try
        {
            var deletedComment = await _geoCommentService.DeleteCommentById(id, userId);
            if (deletedComment is null) return NotFound();

            var returnComment = new CommentReturnV0_2
            {
                Body = new CommentReturnBody
                {
                    Author = deletedComment.AuthorName,
                    Message = deletedComment.Message,
                    Title = deletedComment.Title
                },
                Id = deletedComment.Id,
                Latitude = deletedComment.Latitude,
                Longitude = deletedComment.Longitude
            };

            return Ok(returnComment);
        }
        catch (UnauthorizedException)
        {
            return Unauthorized();
        }

        throw new NotImplementedException();
    }
}
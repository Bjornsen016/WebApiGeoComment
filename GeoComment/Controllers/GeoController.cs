using System.Net;
using AutoMapper;
using GeoComment.DTOs;
using GeoComment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Controllers;

[Route("api/geo-comments")]
[ApiController]
public class GeoController : ControllerBase
{
    private readonly GeoCommentService _geoCommentService;
    private readonly Database _database;

    public GeoController(GeoCommentService geoCommentService, Database database)
    {
        _geoCommentService = geoCommentService;
        _database = database;
    }

    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<CommentReturnDTO>> Get(int id,
        [BindRequired] [FromQuery(Name = "api-version")]
        string apiVersion)
    {
        var comment = await _geoCommentService.GetCommentById(id);

        if (comment == null) return NotFound();

        return Ok(comment);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<List<CommentReturnDTO>>> GetList(
        [BindRequired] double minLon, [BindRequired] double maxLon,
        [BindRequired] double minLat, [BindRequired] double maxLat,
        [BindRequired] [FromQuery(Name = "api-version")]
        string apiVersion)
    {
        var comments = await _geoCommentService.GetComments(minLon, maxLon, minLat, maxLat);
        return Ok(comments);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<CommentReturnDTO>> CreateComment(CommentInputDTO input,
        [BindRequired] [FromQuery(Name = "api-version")]
        string apiVersion)
    {
        try
        {
            var createdComment = await _geoCommentService.CreateCommentInDb(input);
            return CreatedAtAction(nameof(Get), new {id = createdComment.Id}, createdComment);
        }
        catch (AuthorNotFoundException)
        {
            return NotFound("Author not found");
        }
        catch (DbUpdateException)
        {
            return NotFound("A database error occurred");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("/test/reset-db")]
    public async Task<ActionResult> ResetDataBase()
    {
        await _database.RecreateDb();
        return Ok();
    }
}
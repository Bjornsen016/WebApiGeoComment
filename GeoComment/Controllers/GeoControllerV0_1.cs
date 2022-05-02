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
[ControllerName("Geo Controller")]
[ApiVersion("0.1")]
[ApiController]
public class GeoControllerV0_1 : ControllerBase
{
    private readonly GeoCommentService _geoCommentService;
    private readonly IMapper _mapper;

    public GeoControllerV0_1(GeoCommentService geoCommentService, IMapper mapper)
    {
        _geoCommentService = geoCommentService;
        _mapper = mapper;
    }

    [ResponseCache(Duration = 30)]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<CommentReturnDTO>> Get(int id)
    {
        var comment = await _geoCommentService.GetCommentById(id);

        if (comment == null) return NotFound();

        var commentReturn = _mapper.Map<CommentReturnDTO>(comment);

        return Ok(commentReturn);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<List<CommentReturnDTO>>> GetList(
        [BindRequired] double minLon, [BindRequired] double maxLon,
        [BindRequired] double minLat, [BindRequired] double maxLat)
    {
        var comments = await _geoCommentService.GetComments(minLon, maxLon, minLat, maxLat);

        var returnComments = new List<CommentReturnDTO>();
        foreach (var comment in comments)
        {
            var cmt = _mapper.Map<CommentReturnDTO>(comment);
            returnComments.Add(cmt);
        }

        return Ok(returnComments);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<CommentReturnDTO>> CreateComment(CommentInputDTO input)
    {
        try
        {
            var comment = new Comment
            {
                AuthorName = input.Author, Latitude = input.Latitude, Longitude = input.Longitude,
                Message = input.Message, Title = input.Message.Split(" ")[0]
            };
            var createdComment = await _geoCommentService.CreateCommentInDb(comment);
            var commentReturn = _mapper.Map<CommentReturnDTO>(createdComment);

            return CreatedAtAction(nameof(Get), new {id = commentReturn.Id}, commentReturn);
        }
        catch (DbUpdateException)
        {
            return NotFound("A database error occurred");
        }
    }
}
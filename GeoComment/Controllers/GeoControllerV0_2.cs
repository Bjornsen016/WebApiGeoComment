using AutoMapper;
using GeoComment.DTOs;
using GeoComment.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers;

[Route("api/geo-comments")]
[ControllerName("Geo Controller")]
[ApiVersion("0.2")]
[ApiController]
public class GeoControllerV0_2 : ControllerBase
{
    private readonly GeoCommentService _geoCommentService;
    private readonly IMapper _mapper;

    public GeoControllerV0_2(GeoCommentService geoCommentService, IMapper mapper)
    {
        _geoCommentService = geoCommentService;
        _mapper = mapper;
    }

    [Route("{id}")]
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
                Author = comment.Author.UserName,
                Message = comment.Message
            },
            Id = comment.Id,
            Latitude = comment.Latitude,
            Longitude = comment.Longitude
        };

        return Ok(commentReturn);
    }
}
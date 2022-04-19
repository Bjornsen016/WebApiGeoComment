using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GeoComment.Controllers;

[Route("api/geo-comments")]
[ApiController]
public class GeoController : ControllerBase
{
    [Route("{id}")]
    [HttpGet]
    public ActionResult<Comment> Get(int id, [BindRequired] [FromQuery(Name = "api-version")] string apiVersion)
    {
        var comment = new Comment
        {
            Id = 1,
            Author = "Ada",
            Latitude = 10,
            Longitude = 5,
            Message = "Lorem ipsum dolor amet"
        };
        return Ok(comment);
    }

    [HttpGet]
    public IEnumerable<string> GetList(double minLon, double maxLon, double minLat, double maxLat,
        [BindRequired] [FromQuery(Name = "api-version")]
        string apiVersion)
    {
        throw new NotImplementedException();
    }
}
using GeoComment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Database _database;

        public TestController(Database database)
        {
            _database = database;
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
}
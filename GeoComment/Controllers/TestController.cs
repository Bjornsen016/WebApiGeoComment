using GeoComment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers
{
    [Route("test")]
    [ApiVersion("0.1")]
    [ApiVersion("0.2")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Database _database;

        public TestController(Database database)
        {
            _database = database;
        }

        /// <summary>
        /// Resets the database
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success when the database has been reset</response>
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
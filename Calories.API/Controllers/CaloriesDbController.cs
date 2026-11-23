using Calories.APP.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Calories.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaloriesDbController : ControllerBase
    {
        [HttpGet, Route("~/api/SeedDb")]
        public IActionResult Seed()
        {
            var dbFactory = new CaloriesDbFactory();
            dbFactory.SeedDb();
            return Ok("Database seed successful.");
        }
    }
}

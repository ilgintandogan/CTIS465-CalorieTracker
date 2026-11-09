using Calories.APP.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Calories.API.Controllers
{
    // Database Controller for seeding the database with initial data
    // through the Seed action.
    [Route("api/[controller]")] // route for the controller: api/CaloriesDb
    [ApiController] // attribute indicates that this is an API controller
    public class CaloriesDbController : ControllerBase
    {
        [HttpGet, Route("~/api/SeedDb")] // route for the get action
                                         // changed with the Route
                                         // attribute: api/SeedDb
        public IActionResult Seed()
        {
            // Initialize the CaloriesDbFactory instance.
            var dbFactory = new CaloriesDbFactory();
            // Seed the database using the SeedDb method.
            dbFactory.SeedDb();
            // Return HTTP OK status result (200) with a success message.
            return Ok("Database seed successful.");
        }
    }
}

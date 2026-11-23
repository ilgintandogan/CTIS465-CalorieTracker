#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CORE.Models;
using Calories.APP.Features.Meals;

//Generated from Custom Microservices Template.
namespace Calories.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly ILogger<MealsController> _logger;
        private readonly IMediator _mediator;

        public MealsController(ILogger<MealsController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Meals
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator
                    .Send(new MealQueryRequest());
                var list = await response.ToListAsync();
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("MealsGet Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during MealsGet."));
            }
        }

        // GET: api/Meals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator
                    .Send(new MealQueryRequest());
                var item = await response
                    .SingleOrDefaultAsync(r => r.Id == id);
                if (item is not null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("MealsGetById Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during MealsGetById."));
            }
        }

        // POST: api/Meals
        [HttpPost]
        public async Task<IActionResult> Post(MealAddRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        //return CreatedAtAction(nameof(Get),
                        //    new { id = response.Id }, response);
                        return Ok(response);
                    }
                    ModelState.AddModelError("MealsPost", response.Message);
                }
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("MealsPost Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during MealsPost."));
            }
        }

        // PUT: api/Meals
        [HttpPut]
        public async Task<IActionResult> Put(MealUpdateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        //return NoContent();
                        return Ok(response);
                    }
                    ModelState.AddModelError("MealsPut", response.Message);
                }
                return BadRequest(new CommandResponse(
                    false, string.Join("|", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("MealsPut Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during MealsPut."));
            }
        }

        // DELETE: api/Meals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator
                    .Send(new MealRemoveRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    //return NoContent();
                    return Ok(response);
                }
                ModelState.AddModelError("MealsDelete", response.Message);
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("MealsDelete Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during MealsDelete."));
            }
        }
    }
}

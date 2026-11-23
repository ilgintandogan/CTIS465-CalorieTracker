#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CORE.Models;
using Calories.APP.Features.Foods;

//Generated from Custom Microservices Template.
namespace Calories.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly ILogger<FoodsController> _logger;
        private readonly IMediator _mediator;

        public FoodsController(ILogger<FoodsController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Foods
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator
                    .Send(new FoodQueryRequest());
                var list = await response.ToListAsync();
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("FoodsGet Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during FoodsGet."));
            }
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator
                    .Send(new FoodQueryRequest());
                var item = await response
                    .SingleOrDefaultAsync(r => r.Id == id);
                if (item is not null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("FoodsGetById Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during FoodsGetById."));
            }
        }

        // POST: api/Foods
        [HttpPost]
        public async Task<IActionResult> Post(FoodAddRequest request)
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
                    ModelState.AddModelError("FoodsPost", response.Message);
                }
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("FoodsPost Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during FoodsPost."));
            }
        }

        // PUT: api/Foods
        [HttpPut]
        public async Task<IActionResult> Put(FoodUpdateRequest request)
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
                    ModelState.AddModelError("FoodsPut", response.Message);
                }
                return BadRequest(new CommandResponse(
                    false, string.Join("|", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("FoodsPut Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during FoodsPut."));
            }
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator
                    .Send(new FoodRemoveRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    //return NoContent();
                    return Ok(response);
                }
                ModelState.AddModelError("FoodsDelete", response.Message);
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("FoodsDelete Exception: " +
                    exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during FoodsDelete."));
            }
        }
    }
}

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

        // Constructor: injects logger to log the errors to
        // Kestrel Console or Output Window and mediator
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
                // Send a query request to get query response
                var response = await _mediator
                    .Send(new FoodQueryRequest());
                // Convert the query response to a list
                var list = await response.ToListAsync();
                // If there are items, return them with 200 OK
                if (list.Any())
                    return Ok(list);
                // If no items found, return 204 No Content
                return NoContent();
            }
            catch (Exception exception)
            {
                // Log the exception
                _logger.LogError("FoodsGet Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
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
                // Send a query request to get query response
                var response = await _mediator
                    .Send(new FoodQueryRequest());
                // Find the item with the given id
                var item = await response
                    .SingleOrDefaultAsync(r => r.Id == id);
                // If item found, return it with 200 OK
                if (item is not null)
                    return Ok(item);
                // If item not found, return 204 No Content
                return NoContent();
            }
            catch (Exception exception)
            {
                // Log the exception
                _logger.LogError("FoodsGetById Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
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
                // Check if the request model is valid through
                // data annotations
                if (ModelState.IsValid)
                {
                    // Send the create request
                    var response = await _mediator.Send(request);
                    // If creation is successful, return 200 OK with
                    // success command response
                    if (response.IsSuccessful)
                    {
                        //return CreatedAtAction(nameof(Get),
                        //    new { id = response.Id }, response);
                        return Ok(response);
                    }
                    // If creation failed, add error command
                    // response message to model state
                    ModelState.AddModelError("FoodsPost", response.Message);
                }
                // Return 400 Bad Request with all data annotation
                // validation error messages and the error command
                // response message if added seperated by |
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                // Log the exception
                _logger.LogError("FoodsPost Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
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
                // Check if the request model is valid through
                // data annotations
                if (ModelState.IsValid)
                {
                    // Send the update request
                    var response = await _mediator.Send(request);
                    // If update is successful, return 200 OK
                    // with success command response
                    if (response.IsSuccessful)
                    {
                        //return NoContent();
                        return Ok(response);
                    }
                    // If update failed, add error command response
                    // message to model state
                    ModelState.AddModelError("FoodsPut", response.Message);
                }
                // Return 400 Bad Request with all data annotation
                // validation error messages and the error command
                // response message if added seperated by |
                return BadRequest(new CommandResponse(
                    false, string.Join("|", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                // Log the exception
                _logger.LogError("FoodsPut Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
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
                // Send the delete request
                var response = await _mediator
                    .Send(new FoodRemoveRequest() { Id = id });
                // If delete is successful, return 200 OK with
                // success command response
                if (response.IsSuccessful)
                {
                    //return NoContent();
                    return Ok(response);
                }
                // If delete failed, add error command response
                // message to model state
                ModelState.AddModelError("FoodsDelete", response.Message);
                // Return 400 Bad Request with the error
                // command response message
                return BadRequest(new CommandResponse(false,
                    string.Join("|", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                // Log the exception
                _logger.LogError("FoodsDelete Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during FoodsDelete."));
            }
        }
    }
}

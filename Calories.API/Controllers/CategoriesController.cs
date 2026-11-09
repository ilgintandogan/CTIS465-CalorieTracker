#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CORE.Models;
using Calories.APP.Features.Categories;

//Generated from Custom Microservices Template.
namespace Calories.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMediator _mediator;

        // Constructor: injects logger to log the errors to
        // Kestrel Console or Output Window and mediator
        public CategoriesController(ILogger<CategoriesController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Send a query request to get query response
                var response = await _mediator
                    .Send(new CategoryQueryRequest());
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
                _logger.LogError("CategoriesGet Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during CategoriesGet."));
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                // Send a query request to get query response
                var response = await _mediator
                    .Send(new CategoryQueryRequest());
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
                _logger.LogError("CategoriesGetById Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during CategoriesGetById."));
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> Post(CategoryAddRequest request)
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
                    ModelState.AddModelError("CategoriesPost", response.Message);
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
                _logger.LogError("CategoriesPost Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during CategoriesPost."));
            }
        }

        // PUT: api/Categories
        [HttpPut]
        public async Task<IActionResult> Put(CategoryUpdateRequest request)
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
                    ModelState.AddModelError("CategoriesPut", response.Message);
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
                _logger.LogError("CategoriesPut Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during CategoriesPut."));
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Send the delete request
                var response = await _mediator
                    .Send(new CategoryRemoveRequest() { Id = id });
                // If delete is successful, return 200 OK with
                // success command response
                if (response.IsSuccessful)
                {
                    //return NoContent();
                    return Ok(response);
                }
                // If delete failed, add error command response
                // message to model state
                ModelState.AddModelError("CategoriesDelete", response.Message);
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
                _logger.LogError("CategoriesDelete Exception: " +
                    exception.Message);
                // Return 500 Internal Server Error with an error
                // command response with message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommandResponse(false,
                        "An exception occured during CategoriesDelete."));
            }
        }
    }
}

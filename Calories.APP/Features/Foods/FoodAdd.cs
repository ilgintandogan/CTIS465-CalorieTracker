using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Features.Foods
{
    // Request properties are created according to the data that
    // will be retrieved from APIs.
    public class FoodAddRequest : Record, IRequest<CommandResponse>
    {
        // Copy all the non navigation properties from Food entity.
        // Name is required and can be maximum 100 characters.
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string ServingDescription { get; set; }

        /*
        The type changed from int to int? to be able to use [Required]
        attribute. If the type is int, the default value will be 0 and
        the validation will always be successful even if no value is assigned.
        */
        // Minimum value can be 0, maximum value can be 5000.
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 5000, ErrorMessage = "{0} must be between {1} and {2}!")]
        public int? Calories { get; set; }

        // Minimum value can be 0, maximum value can be 1000.
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 1000, ErrorMessage = "{0} must be between {1} and {2}!")]
        public decimal? ProteinGrams { get; set; }

        public bool IsVegan { get; set; }

        // We don't need to get the CreatedDate value from the client
        // since it will be assigned automatically in the handler.

        // Modify (1 to many) or add (many to many) the relationship properties.
        // For foods-category one to many relationship.
        [Required(ErrorMessage = "{0} is required!")]
        public int? CategoryId { get; set; }

        // Since we won't update the relational meal data (MealFoods)
        // through the request, we don't need the MealIds property here.
    }

    // FoodAddHandler inherits the DbService class for Food entity type,
    // therefore Food entity database operations of the DbService
    // class can be used in this class.
    // FoodAddHandler also implements the Mediator IRequestHandler
    // interface for FoodAddRequest and CommandResponse types,
    // which will be used by the related controller's action.
    public class FoodAddHandler : DbService<Food>,
        IRequestHandler<FoodAddRequest, CommandResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public FoodAddHandler(DbContext db) : base(db)
        {
        }

        // Checks whether the food with the same trimmed and
        // case sensitive name exists in the Foods database table
        // first. If it doesn't exist, inserts the Food entity
        // mapped from the FoodAddRequest instance to the Foods
        // database table.
        public async Task<CommandResponse> Handle(
            FoodAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(f =>
                f.Name == request.Name.Trim(), cancellationToken))
                return Error("Food with the same name exists!");
            // f: Food entity delegate.

            // Create a new Food entity instance from the
            // FoodAddRequest instance then insert the entity in the
            // Foods database table (third save default parameter
            // of the DbAddAsync method is true therefore changes of the
            // DbSets are commited to the related database tables).
            var foodEntity = new Food
            {
                Name = request.Name.Trim(),
                ServingDescription = request.ServingDescription?.Trim(),
                // Since request.ServingDescription may be null, ? is used
                // after meaning that if request.ServingDescription is null,
                // assign null to the Food entity's ServingDescription
                // property else assign the trimmed value.
                Calories = request.Calories ?? 0,
                ProteinGrams = request.ProteinGrams ?? 0,
                IsVegan = request.IsVegan,
                CreatedDate = DateTime.Now,
                CategoryId = request.CategoryId ?? 0
                // ??: Null coalescing operator: If request.CategoryId is
                // null assign 0 else assign request.CategoryId value to the
                // Food entity CategoryId property.
            };
            await DbAddAsync(foodEntity, cancellationToken);

            // Entity's Id value will be updated by the database after insert.
            return Success(foodEntity.Id, "Food created successfully.");
        }
    }
}

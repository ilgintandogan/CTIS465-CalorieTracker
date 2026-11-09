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
    public class FoodUpdateRequest : Record, IRequest<CommandResponse>
    {
        // Copy all the non navigation properties from Food entity.
        // Name is required and can be maximum 100 characters.
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ServingDescription { get; set; }

        [Required, Range(0, 5000)]
        public int? Calories { get; set; }

        [Required, Range(0, 1000)]
        public decimal? ProteinGrams { get; set; }

        public bool IsVegan { get; set; }

        // We don't need to get the CreatedDate value from the client
        // since it will be assigned automatically in FoodAddHandler class.

        // Modify (1 to many) or add (many to many) the relationship properties.
        // For foods-category one to many relationship.
        [Required]
        public int? CategoryId { get; set; }
    }

    // FoodUpdateHandler inherits the DbService class for Food entity type,
    // therefore Food entity database operations of the DbService
    // class can be used in this class.
    // FoodUpdateHandler also implements the Mediator IRequestHandler
    // interface for FoodUpdateRequest and CommandResponse types,
    // which will be used by the related controller's action.
    public class FoodUpdateHandler : DbService<Food>,
        IRequestHandler<FoodUpdateRequest, CommandResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public FoodUpdateHandler(DbContext db) : base(db)
        {
        }

        // Checks whether the food with the same trimmed and
        // case sensitive name other than the request record
        // exists in the Foods database table first. If it doesn't exist,
        // gets the Food entity by ID and if found updates the Food
        // entity mapped from the FoodUpdateRequest instance in the Foods
        // database table.
        public async Task<CommandResponse> Handle(FoodUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(f => f.Id != request.Id &&
                f.Name == request.Name.Trim(), cancellationToken))
                return Error("Food with the same name exists!");
            // f: Food entity delegate.

            // Get the Food entity by ID from the Foods database table
            // and check if it is null.
            var foodEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (foodEntity is null)
                return Error("Food not found!");

            // Update the Food entity properties from the
            // FoodUpdateRequest instance then update the entity in the
            // Foods database table (third save default parameter
            // of the DbUpdateAsync method is true therefore changes of the
            // DbSets are commited to the related database tables).
            foodEntity.Name = request.Name.Trim();
            foodEntity.ServingDescription = request.ServingDescription?.Trim();
            foodEntity.Calories = request.Calories ?? 0;
            foodEntity.ProteinGrams = request.ProteinGrams ?? 0;
            foodEntity.IsVegan = request.IsVegan;
            // We don't need to update the CreatedDate value
            // since it will be assigned automatically in the
            // FoodAddHandler class.
            foodEntity.CategoryId = request.CategoryId ?? 0;
            await DbUpdateAsync(foodEntity, cancellationToken);

            return Success(foodEntity.Id, "Food updated successfully.");
        }
    }
}

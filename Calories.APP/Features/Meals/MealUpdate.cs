using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Features.Meals
{
    // Request properties are created according to the data that
    // will be retrieved from APIs.
    public class MealUpdateRequest : Record, IRequest<CommandResponse>
    {
        // Copy all the non navigation properties from Meal entity.
        // Title is required and can be maximum 50 characters.
        [Required, StringLength(50)]
        public string Title { get; set; }

        public MealTypes MealType { get; set; }

        [Required]
        public DateTime? MealDate { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        // Modify (1 to many) or add (many to many) the relationship properties.
        // For meals-foods many to many relationship.
        [Required]
        public List<int> FoodIds { get; set; }
    }

    // MealUpdateHandler inherits the DbService class for Meal entity type,
    // therefore Meal entity database operations of the DbService
    // class can be used in this class.
    // MealUpdateHandler also implements the Mediator IRequestHandler
    // interface for MealUpdateRequest and CommandResponse types,
    // which will be used by the related controller's action.
    public class MealUpdateHandler : DbService<Meal>,
        IRequestHandler<MealUpdateRequest, CommandResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public MealUpdateHandler(DbContext db) : base(db)
        {
        }

        // Override the DbQuery method of the base DbService class to
        // include the MealFood entities to the query. Therefore
        // during update operation, the related MealFood entities
        // will be deleted first and then new related MealFood entities
        // will be inserted through the FoodIds property of the
        // MealUpdateRequest instance.
        protected override IQueryable<Meal> DbQuery()
        {
            return base.DbQuery().Include(m => m.MealFoods);
            // m: Meal entity delegate.
        }

        // Checks whether the meal with the same trimmed and
        // case sensitive title other than the
        // request record exists in the Meals database table first. If it
        // doesn't exist, gets the Meal entity by ID and if found updates the
        // Meal entity mapped from the MealUpdateRequest instance in the
        // Meals database table.
        public async Task<CommandResponse> Handle(MealUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(m => m.Id != request.Id &&
                m.Title == request.Title.Trim(), cancellationToken))
                return Error("Meal with the same title exists!");
            // m: Meal entity delegate.

            // Get the Meal entity by ID from the Meals database table
            // and check if it is null.
            var mealEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (mealEntity is null)
                return Error("Meal not found!");

            // Delete the related MealFood entities first.
            if (request.FoodIds is not null)
                DbRemove(mealEntity.MealFoods);

            // Then update the Meal entity properties from the
            // MealUpdateRequest instance then update the entity in the
            // Meals database table (third save default parameter
            // of the DbUpdateAsync method is true therefore changes of the
            // DbSets are commited to the related database tables).
            mealEntity.Title = request.Title.Trim();
            mealEntity.MealType = request.MealType;
            mealEntity.MealDate = request.MealDate.Value;
            mealEntity.Notes = request.Notes?.Trim();
            mealEntity.FoodIds = request.FoodIds;
            await DbUpdateAsync(mealEntity, cancellationToken);

            return Success(mealEntity.Id, "Meal updated successfully!");
        }
    }
}

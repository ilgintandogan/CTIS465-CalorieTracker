using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Meals
{
    // Delete operation will be performed by the Id property inherited
    // from the Record base class. Therefore, no additional properties
    // are needed to be defined.
    public class MealRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    // MealRemoveHandler class injects the DbContext instance for
    // querying and deleting an existing meal in the Meals database
    // table.
    // The Handle method implemented from the IRequestHandler
    // interface gets the Meal entity by ID first.
    // If found, deletes the Meal entity from the Meals database table.
    public class MealRemoveHandler : DbService<Meal>,
        IRequestHandler<MealRemoveRequest, CommandResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public MealRemoveHandler(DbContext db) : base(db)
        {
        }

        // Override the DbQuery method of the base DbService class to
        // include the MealFood entities to the query. Therefore
        // during delete operation, the related MealFood entities
        // will be deleted first. Then the meal entity will be deleted.
        protected override IQueryable<Meal> DbQuery()
        {
            return base.DbQuery().Include(m => m.MealFoods);
            // m: Meal entity delegate.
        }

        // Deletes an existing meal from the Meals database table
        // with the related MealFood entities if it is found by request's ID.
        public async Task<CommandResponse> Handle(MealRemoveRequest request,
            CancellationToken cancellationToken)
        {
            // Get the Meal entity by ID from the Meals database table
            // and check whether it is null or not.
            var mealEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (mealEntity is null)
                return Error("Meal not found!");

            // Delete the related MealFood entities first.
            DbRemove(mealEntity.MealFoods);

            // Then delete the Meal entity from the Meals database table.
            await DbRemoveAsync(mealEntity, cancellationToken);

            return Success(mealEntity.Id, "Meal deleted successfully.");
        }
    }
}

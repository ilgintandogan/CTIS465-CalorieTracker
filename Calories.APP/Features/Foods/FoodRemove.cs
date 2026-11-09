using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Foods
{
    // Delete operation will be performed by the Id property inherited
    // from the Record base class. Therefore, no additional properties
    // are needed to be defined.
    public class FoodRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    // FoodRemoveHandler class injects the DbContext instance for
    // querying and deleting an existing food in the Foods database
    // table.
    // The Handle method implemented from the IRequestHandler
    // interface gets the Food entity by ID first.
    // If found, deletes the Food entity from the Foods database table.
    public class FoodRemoveHandler : DbService<Food>,
        IRequestHandler<FoodRemoveRequest, CommandResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public FoodRemoveHandler(DbContext db) : base(db)
        {
        }

        // Deletes an existing food from the Foods database table if
        // it is found by request's ID.
        public async Task<CommandResponse> Handle(FoodRemoveRequest request,
            CancellationToken cancellationToken)
        {
            // Get the Food entity by ID from the Foods database table
            // and check whether it is null or not.
            var foodEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (foodEntity is null)
                return Error("Food not found!");

            // Delete the Food entity from the Foods database table
            // (third save default parameter of the DbRemoveAsync method is
            // true therefore changes of the DbSets are commited to
            // the related database tables).
            // Since the delete rule of the Foods and MealFoods relation
            // in the database is Cascade, the relational MealFood entities
            // will be deleted automatically (not recommended).
            await DbRemoveAsync(foodEntity, cancellationToken);

            return Success(foodEntity.Id, "Food deleted successfully.");
        }
    }
}

using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Categories
{
    // Delete operation will be performed by the Id property inherited
    // from the Record base class. Therefore, no additional properties
    // are needed to be defined.
    public class CategoryRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    // CategoryRemoveHandler class injects the DbContext instance for
    // querying and deleting an existing category in the Categories
    // database table.
    // The Handle method implemented from the IRequestHandler
    // interface gets the Category entity by ID first.
    // If found, first checks if there are any related Foods and
    // if none, deletes the Category entity from the Categories database table.
    public class CategoryRemoveHandler : DbService<Category>,
        IRequestHandler<CategoryRemoveRequest, CommandResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public CategoryRemoveHandler(DbContext db) : base(db)
        {
        }

        // Override the DbQuery method to include the related Foods
        // to the Category entity query.
        protected override IQueryable<Category> DbQuery()
        {
            return base.DbQuery().Include(categoryEntity => categoryEntity.Foods);
        }

        // Deletes an existing category from the Categories database table if
        // it is found by request's ID and it has no related foods.
        public async Task<CommandResponse> Handle(CategoryRemoveRequest request,
            CancellationToken cancellationToken)
        {
            // Get the Category entity by ID from the Categories database table
            // and check whether it is null or not.
            var categoryEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (categoryEntity is null)
                return Error("Category not found!");

            // Check if there are any related Foods for the Category entity.
            if (categoryEntity.Foods.Any()) // if (categoryEntity.Foods.Count > 0)
                                            // can also be written.
                return Error("Category has relational foods!");

            // Delete the Category entity from the Categories database table
            // (third save default parameter of the DbRemoveAsync method is
            // true therefore changes of the DbSets are commited to
            // the related database tables).
            await DbRemoveAsync(categoryEntity, cancellationToken);

            return Success(categoryEntity.Id, "Category deleted successfully.");
        }
    }
}

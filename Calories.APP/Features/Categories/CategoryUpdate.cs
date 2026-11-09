using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Features.Categories
{
    // Request properties are created according to the data that
    // will be retrieved from APIs.
    public class CategoryUpdateRequest : Record, IRequest<CommandResponse>
    {
        // Copy all the non navigation properties from Category entity.
        // Name can't be null and can be maximum 50 characters.
        [Required, StringLength(50)]
        public string Name { get; set; }
    }

    // CategoryUpdateHandler class injects the DbContext instance for
    // querying and updating an existing category in the Categories
    // database table.
    // The Handle method implemented from the IRequestHandler
    // interface checks whether the category with the same trimmed and
    // case sensitive name other than the category request record
    // exists in the Categories database table first.
    // If it doesn't exist, updates the Category entity mapped from the
    // CategoryUpdateRequest instance in the Categories database table.
    public class CategoryUpdateHandler : DbService<Category>,
        IRequestHandler<CategoryUpdateRequest, CommandResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public CategoryUpdateHandler(DbContext db) : base(db)
        {
        }

        // Updates an existing category in the Categories database table if a
        // category with the same name other than the category request record
        // doesn't exist.
        public async Task<CommandResponse> Handle(CategoryUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(categoryEntity => categoryEntity.Id != request.Id &&
                    categoryEntity.Name == request.Name.Trim(),
                        cancellationToken))
                return Error("Category with the same name exists!");

            // Get the Category entity by ID from the Categories database table
            // and check whether it is null or not.
            var categoryEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (categoryEntity is null)
                return Error("Category not found!");

            // Update Category entity properties from CategoryUpdateRequest
            // properties. Then update the entity in the Categories database
            // table (third save default parameter of the DbUpdateAsync method
            // is true therefore changes of the DbSets are commited to the
            // related database tables).
            categoryEntity.Name = request.Name.Trim();
            await DbUpdateAsync(categoryEntity, cancellationToken);

            return Success(categoryEntity.Id, "Category updated successfully.");
        }
    }
}

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
    public class CategoryAddRequest : Record, IRequest<CommandResponse>
    {
        // Copy all the non navigation properties from Category entity.
        // Name can't be null and can be maximum 50 characters.
        [Required, StringLength(50)]
        public string Name { get; set; }
    }

    // CategoryAddHandler class injects the DbContext instance for
    // querying and inserting a new category to the Categories database table.
    // The Handle method implemented from the IRequestHandler
    // interface checks whether the category with the same trimmed and
    // case sensitive name exists in the Categories database table first.
    // If it doesn't exist, inserts the Category entity mapped from the
    // CategoryAddRequest instance to the Categories database table.
    public class CategoryAddHandler : DbService<Category>,
        IRequestHandler<CategoryAddRequest, CommandResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public CategoryAddHandler(DbContext db) : base(db)
        {
        }

        // Inserts a new category to the Categories database table if a
        // category with the same name doesn't exist.
        public async Task<CommandResponse> Handle(CategoryAddRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(categoryEntity =>
                    categoryEntity.Name == request.Name.Trim(),
                        cancellationToken))
                return Error("Category with the same name exists!");

            // Create a new Category entity instance from the
            // CategoryAddRequest instance then insert the entity in the
            // Categories database table (third save default parameter of
            // the DbAddAsync method is true therefore changes of the DbSets
            // are commited to the related database tables).
            var categoryEntity = new Category
            {
                Name = request.Name.Trim()
            };
            await DbAddAsync(categoryEntity, cancellationToken);

            // Return a successful command response with the new
            // category ID.
            return Success(categoryEntity.Id, "Category created successfully.");
        }
    }
}

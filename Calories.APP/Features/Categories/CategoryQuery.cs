using Calories.APP.Domain;
using Calories.APP.Features.Foods;
using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Categories
{
    // Request properties are created according to the data that
    // will be retrieved from APIs.
    public class CategoryQueryRequest : Record,
        IRequest<IQueryable<CategoryQueryResponse>>
    {
    }

    // Response properties are created according to the data to be
    // presented in API responses.
    public class CategoryQueryResponse : Record
    {
        // Copy all the non navigation properties from Category entity.
        public string Name { get; set; }

        // Add the new properties ending with R declaring Response
        // for custom properties or formatted string value properties.
        // Way 1: Map primitive type properties for basic information.
        public string FoodNamesR { get; set; }

        // Property value for the count of the foods of the category.
        public int FoodCountR { get; set; }

        // Way 2: Map complex type object for more information.
        // Either Way 1, Way 2 or both can be used.
        public List<FoodQueryResponse> FoodsR { get; set; }
    }

    // CategoryQueryHandler inherits the DbService class for Category entity
    // type, therefore Category entity database operations of the DbService
    // class can be used in this class.
    // CategoryQueryHandler also implements the Mediator IRequestHandler
    // interface for CategoryQueryRequest and IQueryable of
    // CategoryQueryResponse types, which will be used by the related
    // controller's action.
    public class CategoryQueryHandler : DbService<Category>,
        IRequestHandler<CategoryQueryRequest, IQueryable<CategoryQueryResponse>>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public CategoryQueryHandler(DbContext db) : base(db)
        {
        }

        // Overrides the base virtual DbQuery method to include
        // Food entities data to the query to be used in the Handle
        // method and orders the query by Category entity Name
        // property ascending.
        protected override IQueryable<Category> DbQuery()
        {
            return base.DbQuery() // "select * from Categories" entity query.
                .Include(c => c.Foods) // Includes the relational
                                       // Food entities data to the query
                                       // by using left outer join.
                .OrderBy(c => c.Name); // Orders the query by Name
                                       // property of the Category entity
                                       // in ascending order.
            // c: Category entity delegate.
        }

        // Gets the updated query from the DbQuery method then projects
        // the Category entities to category query responses and returns the
        // CategoryQueryResponse query.
        public Task<IQueryable<CategoryQueryResponse>> Handle(
            CategoryQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbQuery().Select(c => new CategoryQueryResponse
            {
                // Map each Category entity property to
                // CategoryQueryResponse property.
                Id = c.Id,
                Name = c.Name,
                // Way 1: Map primitive type data for basic information.
                FoodNamesR = string.Join(", ", c.Foods.Select(f => f.Name)),
                FoodCountR = c.Foods.Count,
                // Way 2: Map complex type object for more information.
                // Either Way 1, Way 2 or both can be used.
                FoodsR = c.Foods.Select(f => new FoodQueryResponse
                {
                    // Map each Food entity property to
                    // CategoryQueryResponse's FoodQueryResponse property.
                    Id = f.Id,
                    Name = f.Name,
                    ServingDescription = f.ServingDescription,
                    Calories = f.Calories,
                    ProteinGrams = f.ProteinGrams,
                    IsVegan = f.IsVegan,
                    CreatedDate = f.CreatedDate,
                    CategoryId = f.CategoryId
                }).ToList()
            });
            // c: Category entity delegate, f: Food entity delegate.

            return Task.FromResult(query);
        }
    }
}

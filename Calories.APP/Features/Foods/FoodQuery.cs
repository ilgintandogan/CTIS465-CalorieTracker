using Calories.APP.Domain;
using Calories.APP.Features.Categories;
using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Foods
{
    // Request properties are created according to the data that
    // will be retrieved from APIs.
    public class FoodQueryRequest : Record,
        IRequest<IQueryable<FoodQueryResponse>>
    {
    }

    // Response properties are created according to the data to be
    // presented in API responses.
    public class FoodQueryResponse : Record
    {
        // Copy all the non navigation properties from Food entity.
        public string Name { get; set; }
        public string ServingDescription { get; set; }
        public int Calories { get; set; }
        public decimal ProteinGrams { get; set; }
        public bool IsVegan { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }

        // Add the new properties ending with R declaring Response
        // for custom properties or formatted string value properties.
        public string ProteinGramsR { get; set; }
        public string IsVeganR { get; set; }
        public string CreatedDateR { get; set; }

        // Way 1:
        // Property for the Name value of the
        // related Category entity.
        public string CategoryNameR { get; set; }
        // Way 2:
        // Property for the mapped CategoryQueryResponse object
        // from the related Category entity.
        // Either Way 1, Way 2 or both can be used.
        public CategoryQueryResponse CategoryR { get; set; }
    }

    // FoodQueryHandler inherits the DbService class for Food entity type,
    // therefore Food entity database operations of the DbService
    // class can be used in this class.
    // FoodQueryHandler also implements the Mediator IRequestHandler
    // interface for FoodQueryRequest and IQueryable of FoodQueryResponse
    // types, which will be used by the related controller's action.
    public class FoodQueryHandler : DbService<Food>,
        IRequestHandler<FoodQueryRequest, IQueryable<FoodQueryResponse>>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public FoodQueryHandler(DbContext db) : base(db)
        {
        }

        // Overrides the base virtual DbQuery method to include
        // Category entities data to the query to be used in the Handle
        // method and orders the query by Food entity Name property
        // ascending first, then the ordered records are ordered
        // descending by Food entity Calories property.
        protected override IQueryable<Food> DbQuery()
        {
            // Relationships:
            // Food <-> Category.
            return base.DbQuery()
                .Include(f => f.Category) // Include Category entities
                                          // from Food entities.
                .OrderBy(f => f.Name)
                .ThenByDescending(f => f.Calories);
            // Only one OrderBy method must be called.
            // The other called ordering methods must be ThenBy.
            // f: Food entity delegate.
        }

        // Gets the updated query from the DbQuery method then projects
        // the Food entities to food query responses and returns the
        // FoodQueryResponse query.
        public Task<IQueryable<FoodQueryResponse>> Handle(
            FoodQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                DbQuery().Select(f => new FoodQueryResponse
                {
                    // Map each Food entity property to FoodQueryResponse property.
                    Id = f.Id,
                    Name = f.Name,
                    ServingDescription = f.ServingDescription,
                    Calories = f.Calories,
                    ProteinGrams = f.ProteinGrams,
                    IsVegan = f.IsVegan,
                    CreatedDate = f.CreatedDate,
                    CategoryId = f.CategoryId,
                    // Way 1: Map primitive type data for basic information.
                    CategoryNameR = f.Category.Name,
                    // Way 2: Map complex type object for more information.
                    // Either Way 1, Way 2 or both can be used.
                    CategoryR = new CategoryQueryResponse
                    {
                        // Map each Category entity property to
                        // FoodQueryResponse's CategoryQueryResponse property.
                        Id = f.Category.Id,
                        Name = f.Category.Name
                    },
                    // Formatted date and time in month/day/year
                    // hour:minute:second format.
                    CreatedDateR = f.CreatedDate.ToString("MM/dd/yyyy HH:mm:ss"),
                    // Ternary operator:
                    IsVeganR = f.IsVegan ? "Yes" : "No",
                    ProteinGramsR = f.ProteinGrams.ToString("N1") // N: Number format.
                                                                  // 1: 1 decimal.
                })); // f: Food entity delegate.
        }
    }
}

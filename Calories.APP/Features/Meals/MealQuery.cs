using Calories.APP.Domain;
using Calories.APP.Features.Foods;
using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Meals
{
    // Request properties are created according to the data that
    // will be retrieved from APIs.
    public class MealQueryRequest : Record,
        IRequest<IQueryable<MealQueryResponse>>
    {
    }

    // Response properties are created according to the data to be
    // presented in API responses.
    public class MealQueryResponse : Record
    {
        // Copy all the non navigation properties from Meal entity.
        public string Title { get; set; }
        public MealTypes MealType { get; set; }
        public DateTime MealDate { get; set; }
        public string Notes { get; set; }

        // Add the new properties ending with R declaring Response
        // for custom properties or formatted string value properties.
        public string MealTypeR { get; set; }
        public string MealDateR { get; set; }

        // Way 1:
        // Property for the concatenated food names
        // of related Food entities.
        public string FoodNamesR { get; set; }

        // Way 2:
        // Property for the FoodQueryResponse objects projected
        // from the related Food entities.
        // Either Way 1, Way 2 or both can be used.
        public List<FoodQueryResponse> FoodsR { get; set; }
    }

    // MealQueryHandler inherits the DbService class for Meal entity type,
    // therefore Meal entity database operations of the DbService
    // class can be used in this class.
    // MealQueryHandler also implements the Mediator IRequestHandler
    // interface for MealQueryRequest and IQueryable of MealQueryResponse
    // types, which will be used by the related controller's action.
    public class MealQueryHandler : DbService<Meal>,
        IRequestHandler<MealQueryRequest, IQueryable<MealQueryResponse>>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public MealQueryHandler(DbContext db) : base(db)
        {
        }

        // Overrides the base virtual DbQuery method to include
        // MealFood and Food entities data to the query to be used in
        // the Handle method and orders the query by Meal entity
        // MealDate property descending first, then the ordered records
        // are ordered ascending by Meal entity Title property.
        protected override IQueryable<Meal> DbQuery()
        {
            // Relationships:
            // Meal <-> MealFoods <-> Foods.
            return base.DbQuery()
                .Include(m => m.MealFoods) // Include MealFood entities
                                           // from Meal entities.
                .ThenInclude(mf => mf.Food) // Then include Food entities
                                            // from MealFood entities.
                .OrderByDescending(m => m.MealDate)
                .ThenBy(m => m.Title);
            // m: Meal entity delegate, mf: MealFood entity delegate.
        }

        // Gets the updated query from the DbQuery method then projects
        // the Meal entities to meal query responses and returns the
        // MealQueryResponse query.
        public Task<IQueryable<MealQueryResponse>> Handle(
            MealQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                DbQuery().Select(m => new MealQueryResponse
                {
                    // Map each Meal entity property to MealQueryResponse property.
                    Id = m.Id,
                    Title = m.Title,
                    MealType = m.MealType,
                    MealDate = m.MealDate,
                    Notes = m.Notes,
                    // Way 1: Map primitive type data for basic information.
                    FoodNamesR = string.Join(", ", m.MealFoods
                        .OrderBy(mf => mf.Food.Name)
                        .Select(mf => mf.Food.Name)),
                    // Way 2: Map complex type object for more information.
                    // Either Way 1, Way 2 or both can be used.
                    FoodsR = m.MealFoods.OrderBy(mf => mf.Food.Name)
                        .Select(mf => new FoodQueryResponse
                        {
                            // Map each Food entity property to
                            // MealQueryResponse's FoodQueryResponse property
                            // through MealFoods.
                            Id = mf.Food.Id,
                            Name = mf.Food.Name,
                            ServingDescription = mf.Food.ServingDescription,
                            Calories = mf.Food.Calories,
                            ProteinGrams = mf.Food.ProteinGrams,
                            IsVegan = mf.Food.IsVegan,
                            CreatedDate = mf.Food.CreatedDate,
                            CategoryId = mf.Food.CategoryId
                        }).ToList(),
                    // Formatted date and time in month/day/year
                    // hour:minute:second format.
                    MealDateR = m.MealDate.ToString("MM/dd/yyyy HH:mm:ss"),
                    MealTypeR = m.MealType.ToString() // Assigns "Breakfast",
                                                      // "Lunch", "Dinner"
                                                      // or "Snack".
                })); // m: Meal entity delegate, mf: MealFood entity delegate.
        }
    }
}

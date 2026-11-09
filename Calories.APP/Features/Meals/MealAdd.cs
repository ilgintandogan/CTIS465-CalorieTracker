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
    public class MealAddRequest : Record, IRequest<CommandResponse>
    {
        // Copy all the non navigation properties from Meal entity.
        // Title is required and can be maximum 50 characters.
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Title { get; set; }

        public MealTypes MealType { get; set; }

        /*
        The type changed from DateTime to DateTime? to be able to use [Required]
        attribute. If the type is DateTime, the default value will be
        DateTime.MinValue and the validation will always be successful even if
        no value is assigned.
        */
        [Required(ErrorMessage = "{0} is required!")]
        public DateTime? MealDate { get; set; }

        [StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Notes { get; set; }

        // Modify (1 to many) or add (many to many) the relationship properties.
        // For meals-foods many to many relationship.
        [Required(ErrorMessage = "At least one food is required!")]
        public List<int> FoodIds { get; set; }
    }

    // MealAddHandler inherits the DbService class for Meal entity type,
    // therefore Meal entity database operations of the DbService
    // class can be used in this class.
    // MealAddHandler also implements the Mediator IRequestHandler
    // interface for MealAddRequest and CommandResponse types,
    // which will be used by the related controller's action.
    public class MealAddHandler : DbService<Meal>,
        IRequestHandler<MealAddRequest, CommandResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public MealAddHandler(DbContext db) : base(db)
        {
        }

        // Checks whether the meal with the same trimmed and
        // case sensitive title exists in the
        // Meals database table first. If it doesn't exist, inserts the
        // Meal entity mapped from the MealAddRequest instance to the Meals
        // database table.
        public async Task<CommandResponse> Handle(
            MealAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(m =>
                m.Title == request.Title.Trim(), cancellationToken))
                return Error("Meal with the same title exists!");
            // m: Meal entity delegate.

            // Create a new Meal entity instance from the
            // MealAddRequest instance then insert the entity in the
            // Meals database table (third save default parameter
            // of the DbAddAsync method is true therefore changes of the
            // DbSets are commited to the related database tables).
            var mealEntity = new Meal
            {
                Title = request.Title.Trim(),
                MealType = request.MealType,
                MealDate = request.MealDate.Value,
                // request.MealDate.Value can be used since
                // request.MealDate is required.
                Notes = request.Notes?.Trim(),
                // Since request.Notes may be null, ? is used after
                // meaning that if request.Notes is null, assign
                // null to the Meal entity's Notes property else
                // assign the trimmed value of request.Notes.
                FoodIds = request.FoodIds
            };
            await DbAddAsync(mealEntity, cancellationToken);

            // Entity's Id value will be updated by the database after insert.
            return Success(mealEntity.Id, "Meal created successfully.");
        }
    }
}

using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Categories
{
    public class CategoryRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class CategoryRemoveHandler : DbService<Category>,
        IRequestHandler<CategoryRemoveRequest, CommandResponse>
    {
        public CategoryRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Category> DbQuery()
        {
            return base.DbQuery().Include(categoryEntity => categoryEntity.Foods);
        }

        public async Task<CommandResponse> Handle(CategoryRemoveRequest request,
            CancellationToken cancellationToken)
        {
            var categoryEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (categoryEntity is null)
                return Error("Category not found!");

            if (categoryEntity.Foods.Any())
                return Error("Category has relational foods!");

            await DbRemoveAsync(categoryEntity, cancellationToken);

            return Success(categoryEntity.Id, "Category deleted successfully.");
        }
    }
}

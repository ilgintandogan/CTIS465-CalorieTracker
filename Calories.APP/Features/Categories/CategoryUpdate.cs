using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Features.Categories
{
    public class CategoryUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
    }

    public class CategoryUpdateHandler : DbService<Category>,
        IRequestHandler<CategoryUpdateRequest, CommandResponse>
    {
        public CategoryUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(CategoryUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(categoryEntity => categoryEntity.Id != request.Id &&
                    categoryEntity.Name == request.Name.Trim(),
                        cancellationToken))
                return Error("Category with the same name exists!");

            var categoryEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (categoryEntity is null)
                return Error("Category not found!");

            categoryEntity.Name = request.Name.Trim();
            await DbUpdateAsync(categoryEntity, cancellationToken);

            return Success(categoryEntity.Id, "Category updated successfully.");
        }
    }
}

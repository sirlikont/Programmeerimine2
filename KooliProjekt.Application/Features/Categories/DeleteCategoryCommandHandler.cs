using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Categories
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteCategoryCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var category = await _dbContext.Categories.FindAsync(request.Id);

            if (category == null)
            {
                result.AddError("Category not found.");
                return result;
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}

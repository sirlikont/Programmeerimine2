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
    public class SaveCategoryCommandHandler : IRequestHandler<SaveCategoryCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveCategoryCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(SaveCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            // Kui Id on 0, siis lisa uus kategooria
            var category = request.Id == 0
                ? new Category()
                : await _dbContext.Categories.FindAsync(request.Id);

            category.Name = request.Name;

            if (request.Id == 0)
            {
                await _dbContext.Categories.AddAsync(category, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
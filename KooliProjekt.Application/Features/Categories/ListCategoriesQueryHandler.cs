using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Categories
{
    public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, OperationResult<IList<Category>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListCategoriesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Category>>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Category>>();
            result.Value = await _dbContext.Categories
                                           .Include(c => c.Products)
                                           .ToListAsync();
            return result;
        }
    }
}


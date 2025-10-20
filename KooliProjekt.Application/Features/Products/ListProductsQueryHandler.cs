using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Products
{
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, OperationResult<IList<Product>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListProductsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Product>>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Product>>();

            // Loeme Products tabelist ja võtame kaasa Category info
            result.Value = await _dbContext.Products
                                           .Include(p => p.Category)
                                           .ToListAsync();

            return result;
        }
    }
}

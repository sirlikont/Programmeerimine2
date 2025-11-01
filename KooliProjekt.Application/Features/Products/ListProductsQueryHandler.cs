using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Products
{
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, OperationResult<PagedResult<Product>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListProductsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Product>>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Product>>();

            result.Value = await _dbContext.Products
                                           .OrderBy(p => p.Name)
                                           .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }

    }
}

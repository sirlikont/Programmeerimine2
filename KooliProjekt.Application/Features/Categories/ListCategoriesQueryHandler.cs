using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Categories
{
    public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, OperationResult<PagedResult<Category>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListCategoriesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<Category>>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Category>>();

            result.Value = await _dbContext.Categories
                                           .OrderBy(c => c.Name) // Võid muuta vastavalt
                                           .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
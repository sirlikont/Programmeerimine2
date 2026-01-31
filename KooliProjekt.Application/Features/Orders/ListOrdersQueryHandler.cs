using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Orders
{
    public class ListOrdersQueryHandler : IRequestHandler<ListOrdersQuery, OperationResult<PagedResult<Order>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListOrdersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<Order>>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Order>>();

            result.Value = await _dbContext.Orders
                                           .OrderBy(o => o.Id) // Võid muuta vastavalt
                                           .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}

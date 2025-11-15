using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class ListOrderItemsQueryHandler : IRequestHandler<ListOrderItemsQuery, OperationResult<PagedResult<OrderItem>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListOrderItemsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<OrderItem>>> Handle(ListOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<OrderItem>>();

            result.Value = await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .OrderBy(oi => oi.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}

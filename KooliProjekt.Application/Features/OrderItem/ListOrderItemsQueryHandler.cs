using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Products;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                                           .OrderBy(p => p.Id)
                                           .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}

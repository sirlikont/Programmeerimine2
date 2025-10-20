using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Orders
{
    public class ListOrdersQueryHandler : IRequestHandler<ListOrdersQuery, OperationResult<IList<Order>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListOrdersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Order>>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Order>>();

            // Loeme kõik Orders ja Include kaudu ka OrderItems ning nende Products
            result.Value = await _dbContext.Orders
                                           .Include(o => o.OrderItems)
                                           .ThenInclude(oi => oi.Product)
                                           .ToListAsync();

            return result;
        }
    }
}
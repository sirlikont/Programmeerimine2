using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Orders
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetOrderQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext.Orders
                .Include(o => o.OrderItems) // Siin lisame ka itemid
                .Where(o => o.Id == request.Id)
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.Status,
                    Items = o.OrderItems.Select(oi => new
                    {
                        oi.Id,
                        oi.ProductId,
                        oi.Quantity,
                        oi.PriceAtOrder
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}


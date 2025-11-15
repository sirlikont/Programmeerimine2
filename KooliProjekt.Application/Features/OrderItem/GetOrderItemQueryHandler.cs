using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class GetOrderItemQueryHandler : IRequestHandler<GetOrderItemQuery, OperationResult<OrderItem>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetOrderItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<OrderItem>> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<OrderItem>();

            result.Value = await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == request.Id, cancellationToken);

            return result;
        }
    }
}


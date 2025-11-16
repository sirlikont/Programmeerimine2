using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Products;
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
    public class GetOrderItemQueryHandler : IRequestHandler<GetOrderItemQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetOrderItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext.OrderItems
                .Where(oi => oi.Id == request.Id)
                .Select(oi => new
                {
                    oi.Id,
                    oi.Quantity,
                    oi.PriceAtOrder,

                    Product = new
                    {
                        oi.Product.Id,
                        oi.Product.Name,
                        oi.Product.Price
                    },

                    Order = new
                    {
                        oi.Order.Id,
                        oi.Order.OrderDate,
                        oi.Order.Status
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}


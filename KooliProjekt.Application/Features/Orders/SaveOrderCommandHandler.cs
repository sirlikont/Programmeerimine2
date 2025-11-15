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
    public class SaveOrderCommandHandler : IRequestHandler<SaveOrderCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveOrderCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveOrderCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            Order order;

            if (request.Id == 0)
            {
                order = new Order();
                await _dbContext.Orders.AddAsync(order, cancellationToken);
            }
            else
            {
                order = await _dbContext.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
            }

            order.OrderDate = request.OrderDate;
            order.Status = request.Status;

            // Eemaldame olemasolevad OrderItems ja lisame uuesti
            order.OrderItems.Clear();
            order.OrderItems.AddRange(request.OrderItems.Select(oi => new KooliProjekt.Application.Data.OrderItem
            {
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                PriceAtOrder = oi.PriceAtOrder
            }));


            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

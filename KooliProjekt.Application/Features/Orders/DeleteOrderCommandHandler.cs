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
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteOrderCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Leia order koos order itemitega
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null)
            {
                result.AddError("Order not found.");
                return result;
            }

            // Kustuta orderi itemid
            _dbContext.OrderItems.RemoveRange(order.OrderItems);

            // Kustuta order
            _dbContext.Orders.Remove(order);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}

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

namespace KooliProjekt.Application.Features.OrderItems
{
    public class SaveOrderItemCommandHandler : IRequestHandler<SaveOrderItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveOrderItemCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveOrderItemCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            KooliProjekt.Application.Data.OrderItem item;

            if (request.Id == 0)
            {
                // Uue order itemi loomine
                item = new KooliProjekt.Application.Data.OrderItem();
                await _dbContext.OrderItems.AddAsync(item, cancellationToken);
            }
            else
            {
                // Olemasoleva order itemi leidmine
                item = await _dbContext.OrderItems
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            }

            // Andmete määramine
            item.ProductId = request.ProductId;
            item.OrderId = request.OrderId;
            item.Quantity = request.Quantity;
            item.PriceAtOrder = request.PriceAtOrder;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}


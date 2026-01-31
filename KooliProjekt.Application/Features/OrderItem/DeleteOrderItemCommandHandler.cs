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
    public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteOrderItemCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            var item = await _dbContext.OrderItems
                .FirstOrDefaultAsync(oi => oi.Id == request.Id, cancellationToken);

            if (item != null)
            {
                _dbContext.OrderItems.Remove(item);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProductCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var product = await _dbContext.Products.FindAsync(request.Id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }

            return result;
        }
    }
}

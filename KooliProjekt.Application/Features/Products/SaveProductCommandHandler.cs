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
    public class SaveProductCommandHandler : IRequestHandler<SaveProductCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProductCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProductCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var product = request.Id == 0
                ? new Product()
                : await _dbContext.Products.FindAsync(request.Id);

            if (product == null)
                product = new Product();

            product.Name = request.Name;
            product.Description = request.Description;
            product.PhotoUrl = request.PhotoUrl;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            if (request.Id == 0)
                await _dbContext.Products.AddAsync(product);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}

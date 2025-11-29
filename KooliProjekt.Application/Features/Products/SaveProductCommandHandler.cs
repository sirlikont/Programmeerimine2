using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class SaveProductCommandHandler : IRequestHandler<SaveProductCommand, OperationResult>
    {
        private readonly IProductRepository _productRepository;

        public SaveProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<OperationResult> Handle(SaveProductCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var product = request.Id != 0
                ? await _productRepository.GetByIdAsync(request.Id)
                : new Product();

            product.Name = request.Name;
            product.Description = request.Description;
            product.PhotoUrl = request.PhotoUrl;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            await _productRepository.SaveAsync(product);

            return result;
        }
    }
}

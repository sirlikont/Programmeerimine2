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
using KooliProjekt.Application.Data.Repositories;

namespace KooliProjekt.Application.Features.Products
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, OperationResult<object>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<OperationResult<object>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var product = await _productRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PhotoUrl = product.PhotoUrl,
                Price = product.Price,
                CategoryId = product.CategoryId
            };

            return result;
        }
    }
}


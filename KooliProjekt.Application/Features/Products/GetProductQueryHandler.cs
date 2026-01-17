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
using KooliProjekt.Application.Dto;

namespace KooliProjekt.Application.Features.Products
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, OperationResult<ProductDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetProductQueryHandler(ApplicationDbContext dbContext)
        {
            // Kui dbContext on null, viskame ArgumentNullException
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<ProductDto>();

            if (request == null)
            {
                result.Value = null;
                return result;
            }

            var product = await _dbContext.Products
                .Where(p => p.Id == request.Id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    PhotoUrl = p.PhotoUrl,
                    CategoryId = p.CategoryId
                })
                .FirstOrDefaultAsync(cancellationToken);

            result.Value = product;
            return result;
        }
    }
}

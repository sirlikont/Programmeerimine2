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

namespace KooliProjekt.Application.Features.Products
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetProductQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext.Products
                .Include(p => p.Category)
                .Where(p => p.Id == request.Id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.PhotoUrl,
                    p.Price,
                    Category = new
                    {
                        p.Category.Id,
                        p.Category.Name
                    }
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}

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

namespace KooliProjekt.Application.Features.Categories
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCategoryQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Categories
                .Where(cat => cat.Id == request.Id)
                .Select(cat => new
                {
                    Id = cat.Id,
                    Name = cat.Name
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}

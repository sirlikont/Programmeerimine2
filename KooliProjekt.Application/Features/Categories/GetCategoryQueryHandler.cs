using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Categories
{
    public class GetCategoryQueryHandler
        : IRequestHandler<GetCategoryQuery, OperationResult<CategoryDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCategoryQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<CategoryDto>> Handle(
            GetCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<CategoryDto>();

            if (request == null)
            {
                result.Value = null;
                return result;
            }

            result.Value = await _dbContext
                .Categories
                .Where(cat => cat.Id == request.Id)
                .Select(cat => new CategoryDto
                {
                    Id = cat.Id,
                    Name = cat.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
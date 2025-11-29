using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Categories
{
    // 28.11
    // Kasutab ICategoryRepositoryt
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, OperationResult<object>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<OperationResult<object>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var category = await _categoryRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = category.Id,
                Name = category.Name,
                // kui tahad hiljem: Products = category.Products.Select(...)
            };

            return result;
        }
    }
}

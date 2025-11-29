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

namespace KooliProjekt.Application.Features.Categories
{
    public class SaveCategoryCommandHandler : IRequestHandler<SaveCategoryCommand, OperationResult>
    {
        private readonly ICategoryRepository _categoryRepository;

        public SaveCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<OperationResult> Handle(SaveCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Kui Id on 0, siis lisa uus kategooria
            var category = request.Id != 0
                ? await _categoryRepository.GetByIdAsync(request.Id)
                : new Category();

            category.Name = request.Name;

            await _categoryRepository.SaveAsync(category);

            return result;
        }
    }
}
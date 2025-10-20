using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Categories
{
    public class ListCategoriesQuery : IRequest<OperationResult<IList<Category>>>
    {
    }
}

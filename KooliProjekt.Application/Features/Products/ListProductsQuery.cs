using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Products
{
    public class ListProductsQuery : IRequest<OperationResult<IList<Product>>>
    {
    }
}
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class ListProductsQuery : IRequest<OperationResult<PagedResult<Product>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
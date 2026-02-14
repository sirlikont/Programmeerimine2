using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Orders
{
    [ExcludeFromCodeCoverage]
    public class ListOrdersQuery : IRequest<OperationResult<PagedResult<Order>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
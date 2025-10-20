using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Orders
{
    // IRequest<OperationResult<IList<Order>>> tähendab, et päring tagastab OperationResult tüüpi listi Order objektidest
    public class ListOrdersQuery : IRequest<OperationResult<IList<Order>>>
    {
    }
}
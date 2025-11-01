﻿using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Orders
{
    public class ListOrdersQuery : IRequest<OperationResult<PagedResult<Order>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Orders
{
    [ExcludeFromCodeCoverage]
    public class GetOrderQuery : IRequest<OperationResult<OrderDto>>
    {
        public int Id { get; set; }
    }
}

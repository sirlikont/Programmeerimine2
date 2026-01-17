using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using KooliProjekt.Application.Dto;

namespace KooliProjekt.Application.Features.Orders
{
    public class GetOrderQuery : IRequest<OperationResult<OrderDto>>
    {
        public int Id { get; set; }
    }
}

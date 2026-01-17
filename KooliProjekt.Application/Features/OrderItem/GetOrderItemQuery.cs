using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using KooliProjekt.Application.Dto;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class GetOrderItemQuery : IRequest<OperationResult<OrderItemDto>>
    {
        public int Id { get; set; }
    }
}
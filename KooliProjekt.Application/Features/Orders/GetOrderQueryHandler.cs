using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data.Repositories;

namespace KooliProjekt.Application.Features.Orders
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OperationResult<object>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OperationResult<object>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var order = await _orderRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status
                // jätame seotud OrderItems välja
            };

            return result;
        }
    }
}

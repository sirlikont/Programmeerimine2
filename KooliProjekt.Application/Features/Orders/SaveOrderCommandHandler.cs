using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Orders
{
    public class SaveOrderCommandHandler : IRequestHandler<SaveOrderCommand, OperationResult>
    {
        private readonly IOrderRepository _orderRepository;

        public SaveOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OperationResult> Handle(SaveOrderCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var order = request.Id != 0
                ? await _orderRepository.GetByIdAsync(request.Id)
                : new Order();

            order.OrderDate = request.OrderDate;
            order.Status = request.Status;

            await _orderRepository.SaveAsync(order);

            return result;
        }
    }
}

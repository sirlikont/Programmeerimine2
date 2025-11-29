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

namespace KooliProjekt.Application.Features.OrderItems
{
    public class SaveOrderItemCommandHandler : IRequestHandler<SaveOrderItemCommand, OperationResult>
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public SaveOrderItemCommandHandler(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<OperationResult> Handle(SaveOrderItemCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var orderItem = request.Id != 0
                ? await _orderItemRepository.GetByIdAsync(request.Id)
                : new OrderItem();

            orderItem.ProductId = request.ProductId;
            orderItem.OrderId = request.OrderId;
            orderItem.Quantity = request.Quantity;
            orderItem.PriceAtOrder = request.PriceAtOrder;

            await _orderItemRepository.SaveAsync(orderItem);

            return result;
        }
    }
}

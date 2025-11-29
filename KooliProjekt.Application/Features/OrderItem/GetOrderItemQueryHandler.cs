using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Products;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class GetOrderItemQueryHandler : IRequestHandler<GetOrderItemQuery, OperationResult<object>>
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public GetOrderItemQueryHandler(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<OperationResult<object>> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var orderItem = await _orderItemRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = orderItem.Id,
                ProductId = orderItem.ProductId,
                OrderId = orderItem.OrderId,
                Quantity = orderItem.Quantity,
                PriceAtOrder = orderItem.PriceAtOrder
                // jätame seotud objektid välja (Product, Order)
            };

            return result;
        }
    }
}


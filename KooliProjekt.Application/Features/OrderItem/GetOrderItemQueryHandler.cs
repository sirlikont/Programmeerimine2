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
using KooliProjekt.Application.Dto;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class GetOrderItemQueryHandler : IRequestHandler<GetOrderItemQuery, OperationResult<OrderItemDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetOrderItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<OrderItemDto>> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<OrderItemDto>();

            if (request == null)
            {
                result.Value = null;
                return result;
            }

            var orderItem = await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Id == request.Id)
                .Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    PriceAtOrder = oi.PriceAtOrder,
                    Product = new ProductDetailsDto
                    {
                        Id = oi.Product.Id,
                        Name = oi.Product.Name,
                        Description = oi.Product.Description,
                        PhotoUrl = oi.Product.PhotoUrl,
                        Price = oi.Product.Price
                    },
                    Order = new OrderDetailsDto
                    {
                        Id = oi.Order.Id,
                        OrderDate = oi.Order.OrderDate,
                        Status = oi.Order.Status
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            result.Value = orderItem;
            return result;
        }
    }
}
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
using KooliProjekt.Application.Dto;

namespace KooliProjekt.Application.Features.Orders
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OperationResult<OrderDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetOrderQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<OrderDto>();

            if (request == null)
            {
                result.Value = null;
                return result;
            }

            result.Value = await _dbContext.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.Id == request.Id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    Items = o.OrderItems.Select(oi => new OrderItemDto
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
                            Id = o.Id,
                            OrderDate = o.OrderDate,
                            Status = o.Status
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
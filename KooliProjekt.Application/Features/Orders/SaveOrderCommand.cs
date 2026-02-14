using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
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
    public class SaveOrderCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();


        public class OrderItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal PriceAtOrder { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using KooliProjekt.Application.Dto;

namespace KooliProjekt.Application.Features.Products
{
    public class GetProductQuery : IRequest<OperationResult<ProductDto>>
    {
        public int Id { get; set; }
    }
}

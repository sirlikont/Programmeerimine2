using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Categories
{
    public class DeleteCategoryCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}

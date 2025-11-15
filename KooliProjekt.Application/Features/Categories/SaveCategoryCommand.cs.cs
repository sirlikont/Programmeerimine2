using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Categories
{
    public class SaveCategoryCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; } // 0 = uus kategooria
        public string Name { get; set; }
    }
}


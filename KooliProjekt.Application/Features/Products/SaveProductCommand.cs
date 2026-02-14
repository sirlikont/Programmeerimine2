using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Products
{
    [ExcludeFromCodeCoverage]
    public class SaveProductCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }           // 0 kui uus
        public string Name { get; set; }      // Nimi
        public string Description { get; set; } // Kirjeldus
        public string PhotoUrl { get; set; }  // Foto URL
        public decimal Price { get; set; }    // Hind
        public int CategoryId { get; set; }   // Kategooria
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Dto
{
    // DTO toote info jaoks OrderItemis
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Price { get; set; }
    }

    // DTO tellimuse info jaoks OrderItemis
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }

    // Peamine OrderItem DTO
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }

        // Nüüd viidatakse ümbernimetatud DTOdele
        public ProductDetailsDto Product { get; set; }
        public OrderDetailsDto Order { get; set; }
    }
}


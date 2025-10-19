using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Seos kategooriaga (üks toode kuulub ühte kategooriasse)
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KooliProjekt.Application.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Seos toodetega (üks kategooria -> mitu toodet)
        public List<Product> Products { get; set; } = new();
    }
}
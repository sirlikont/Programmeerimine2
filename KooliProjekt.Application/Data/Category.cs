using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KooliProjekt.Application.Data
{
    public class Category : Entity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Seos toodetega (üks kategooria -> mitu toodet)
        public List<Product> Products { get; set; } = new();
    }
}

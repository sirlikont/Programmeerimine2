using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    // Category repository klass
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        // BaseRepository ei tea, et Get peab tooma kaasa ka Products
        public override async Task<Category> GetByIdAsync(int id)
        {
            return await DbContext
                .Categories
                .Include(c => c.Products)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
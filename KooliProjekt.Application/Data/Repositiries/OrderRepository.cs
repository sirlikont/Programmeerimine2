using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data.Repositories;

namespace KooliProjekt.Application.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<Order> GetByIdAsync(int id)
        {
            return await DbContext
                .Orders
                .Include(o => o.OrderItems)  // Include OrderItems
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}

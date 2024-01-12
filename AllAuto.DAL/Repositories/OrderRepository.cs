using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;

namespace AllAuto.DAL.Repositories
{
    public class OrderRepository : IBaseRepository<Order>
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Order entity)
        {
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Order entity)
        {
            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Order> GetAll()
        {
            return _context.Orders;
        }

        public async Task<Order> Update(Order entity)
        {
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

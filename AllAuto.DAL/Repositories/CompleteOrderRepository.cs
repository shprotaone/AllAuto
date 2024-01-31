using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;

namespace AllAuto.DAL.Repositories
{
    public class CompleteOrderRepository : IBaseRepository<CompleteOrder>
    {
        private readonly ApplicationDbContext _context;

        public CompleteOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public async Task<bool> Create(CompleteOrder entity)
        {
            await _context.CompleteOrders.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return true;

        }

        public async Task<bool> Delete(CompleteOrder entity)
        {
            _context.CompleteOrders.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<CompleteOrder> GetAll()
        {
            return _context.CompleteOrders;
        }

        public async Task<CompleteOrder> Update(CompleteOrder entity)
        {
            _context.CompleteOrders.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

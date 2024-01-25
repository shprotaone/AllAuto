using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;

namespace AllAuto.DAL.Repositories
{
    public class BasketRepository : IBaseRepository<Basket>
    {
        private readonly ApplicationDbContext _context;

        public BasketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Basket entity)
        {
            await _context.Baskets.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Basket entity)
        {
             _context.Baskets.Remove(entity);
            await _context.SaveChangesAsync();
            return true;

        }

        public IQueryable<Basket> GetAll()
        {
            return _context.Baskets;
        }

        public async Task<Basket> Update(Basket entity)
        {
            _context.Baskets.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

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

        public Task<bool> Create(Basket entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Basket entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Basket> GetAll()
        {
            return _context.Baskets;
        }

        public Task<Basket> Update(Basket entity)
        {
            throw new NotImplementedException();
        }
    }
}

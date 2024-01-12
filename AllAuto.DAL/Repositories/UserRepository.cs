
using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;

namespace AllAuto.DAL.Repositories
{
    public class UserRepository : IBaseRepository<User>
    {

        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<User> GetAll()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<User> Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            return entity;

        }
    }
}

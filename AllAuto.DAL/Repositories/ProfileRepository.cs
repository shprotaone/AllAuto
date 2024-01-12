using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;

namespace AllAuto.DAL.Repositories
{
    public class ProfileRepository : IBaseRepository<Profile>
    {
        private readonly ApplicationDbContext _context;

        public ProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Profile entity)
        {
            await _context.Profiles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Profile entity)
        {
            _context.Profiles.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Profile> GetAll()
        {
            return _context.Profiles;
        }

        public async Task<Profile> Update(Profile entity)
        {
            _context.Profiles.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

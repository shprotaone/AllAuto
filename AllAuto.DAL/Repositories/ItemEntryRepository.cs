using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;

namespace AllAuto.DAL.Repositories
{
    public class ItemEntryRepository : IBaseRepository<ItemEntry>
    {
        private readonly ApplicationDbContext _context;

        public ItemEntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(ItemEntry entity)
        {
            await _context.ItemEntries.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(ItemEntry entity)
        {
            _context.ItemEntries.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<ItemEntry> GetAll()
        {
            return _context.ItemEntries;
        }

        public async Task<ItemEntry> Update(ItemEntry entity)
        {
            _context.ItemEntries.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

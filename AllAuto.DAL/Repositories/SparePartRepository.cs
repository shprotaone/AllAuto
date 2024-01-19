using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AllAuto.DAL.Repositories
{
    public class SparePartRepository : IBaseRepository<SparePart>
    {
        //Взаимодействие с БД
        //TODO: Добавить бд по запчастям
        private readonly ApplicationDbContext _context;

        public SparePartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(SparePart entity)
        {
            await _context.SpareParts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(SparePart entity)
        {
            _context.SpareParts.Remove(entity);
            await _context.SaveChangesAsync();
            
            return true;

        }

        public IQueryable<SparePart> GetAll()
        {
            return _context.SpareParts.AsQueryable();
        }

        public async Task<SparePart> Update(SparePart car)
        {
            _context.SpareParts.Update(car);
            await _context.SaveChangesAsync();

            return car;

        }
    }
}

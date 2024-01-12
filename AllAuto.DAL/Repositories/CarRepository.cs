using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AllAuto.DAL.Repositories
{
    public class CarRepository : IBaseRepository<Car>
    {
        //Взаимодействие с БД
        //TODO: Добавить бд по запчастям
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Car entity)
        {
            await _context.Car.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Car entity)
        {
            _context.Car.Remove(entity);
            await _context.SaveChangesAsync();
            
            return true;

        }

        public IQueryable<Car> GetAll()
        {
            return _context.Car.AsQueryable();
        }

        public async Task<Car> Update(Car car)
        {
            _context.Car.Update(car);
            await _context.SaveChangesAsync();

            return car;

        }
    }
}

using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AllAuto.Service.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Car> _carRepository;

        public BasketService(IBaseRepository<User> userRepository, IBaseRepository<Car> carRepository)
        {
            _userRepository = userRepository;
            _carRepository = carRepository;
        }

        public async Task<BaseResponse<IEnumerable<OrderViewModel>>> GetItems(string name)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Orders)
                    .FirstOrDefaultAsync(x => x.Name == name);

                if (user == null)
                {
                    return new BaseResponse<IEnumerable<OrderViewModel>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                var orders = user.Basket?.Orders;
                var response = from p in orders
                               join c in _carRepository.GetAll() on p.CardId equals c.Id
                               select new OrderViewModel
                               {
                                   Id = p.Id,
                                   CarName = c.Name,
                                   MaxSpeed = c.MaxSpeed,
                                   Model = c.Model,
                                   Image = c.Avatar,
                                   DateCreate = c.DateCreate.ToString(),
                               };

                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Data = response,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<OrderViewModel>> GetItem(string? name, long id)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x => x.Basket) // ленивая загрузка, Lazy load
                    .ThenInclude(x => x.Orders)
                    .FirstOrDefaultAsync(x => x.Name == name);

                if (user == null)
                {
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                var orders = user.Basket?.Orders.Where(x => x.Id == id).ToList();
                if (orders == null || orders.Count == 0)
                {
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = "Заказов нет",
                        StatusCode = Domain.Enum.StatusCode.OrderNotFound
                    };
                }

                var response =
                    (from p in orders
                     join c in _carRepository.GetAll() on p.CardId equals c.Id
                     select new OrderViewModel()
                     {
                         Id = c.Id,
                         CarName = c.Name,
                         MaxSpeed = c.MaxSpeed,
                         Model = c.Model,
                         Image = c.Avatar,
                         DateCreate = p.DateCreated.ToString(),

                     }).FirstOrDefault();

                return new BaseResponse<OrderViewModel>()
                {
                    Data = response,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }        
    }
}

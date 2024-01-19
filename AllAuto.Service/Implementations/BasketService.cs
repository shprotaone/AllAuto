using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;
using AllAuto.Domain.ViewModels.SparePart;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AllAuto.Service.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<SparePart> _sparePartRepository;

        public BasketService(IBaseRepository<User> userRepository, 
            IBaseRepository<SparePart> sparePartRepository)
        {
            _userRepository = userRepository;
            _sparePartRepository = sparePartRepository;
        }

        public async Task<BaseResponse<IEnumerable<OrderViewModel>>> GetItems(string name)
        {
            try
            {
               User user = FindUser(name).Result;

                if (user == null)
                {
                    return new BaseResponse<IEnumerable<OrderViewModel>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                IEnumerable<OrderViewModel> orders = FindOrders(user);

                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Data = orders,
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

                return new BaseResponse<OrderViewModel>()
                {
                    Data = new OrderViewModel(),        //заглушка
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

        public async Task<BaseResponse<OrderViewModel>> AddItem(string userName, long id, int Amount)
        {
            try
            {
                //найти юзера
                User user = FindUser(userName).Result;

                if (user == null)
                {
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                //взять экземпляр корзины
                Basket basket = FindBasket(user).Result;
                Order order = FindOrder(user);
                //добавит указаный товар в корзину

            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }

            return new BaseResponse<OrderViewModel>()
            {
                Description = "Заглушка",
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }

        private async Task<User> FindUser(string name)
        {
            User? user = await _userRepository.GetAll()
                   .Include(x => x.Basket)
                   .ThenInclude(x => x.Orders)
                   .FirstOrDefaultAsync(x => x.Name == name);

            return user;
        }

        private Order FindOrder(User user)
        {
            Order? order = user.Basket?.Orders.FirstOrDefault(x => !x.IsClosed);
            if (order == null)
            {
                order = new Order()
                {
                    DateCreated = DateTime.Now,
                };
            }

            return order;
        }

        private IEnumerable<OrderViewModel> FindOrders(User user)
        {
            List<OrderViewModel>? order = ConvertToOrderView(user.Basket?.Orders.ToList());
                return order;
        }

        private async Task<Basket> FindBasket(User user)
        {
            return user.Basket;
        }

        private List<OrderViewModel> ConvertToOrderView(List<Order> orders)
        {
            List<OrderViewModel> response = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                order.SparePart = _sparePartRepository.GetAll()
                    .FirstOrDefault(x => x.Id == order.SparePartId);

                response.Add(new OrderViewModel()
                {
                    Id = order.Id,
                    Name = order.SparePart.Name,
                    Manufactor = order.SparePart.Model,
                    Price = order.SparePart.Price,
                    DateCreate = order.SparePart.DateCreate.ToString(),                    
                });
            }

            return response;
        }
    }
}
